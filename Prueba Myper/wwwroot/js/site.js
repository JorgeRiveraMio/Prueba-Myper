
document.addEventListener("DOMContentLoaded", function () {
    // Mapeo de elementos
    const selects = {
        crear: {
            departamento: document.getElementById("departamento"),
            provincia: document.getElementById("provincia"),
            distrito: document.getElementById("distrito")
        },
        editar: {
            departamento: document.getElementById("editarDepartamento"),
            provincia: document.getElementById("editarProvincia"),
            distrito: document.getElementById("editarDistrito")
        }
    };

    // Función para poblar un select con opciones
    function poblarSelect(select, data, textKey, valueKey) {
        select.innerHTML = "<option value=''>-- Selecciona --</option>";
        data.forEach(item => {
            const option = document.createElement("option");
            option.value = item[valueKey];
            option.textContent = item[textKey];
            select.appendChild(option);
        });
    }
    // Función para cargar provincias o distritos
    function cargarUbigeo(url, select, textKey, valueKey, callback) {
        fetch(url)
            .then(res => res.json())
            .then(data => {
                poblarSelect(select, data, textKey, valueKey);
                if (typeof callback === "function") callback();
            });
    }

    // Listener en cascada para crear o editar
    function configurarUbigeoCascada(deps) {
        deps.departamento.addEventListener("change", () => {
            const id = deps.departamento.value;
            poblarSelect(deps.provincia, [], "", "");
            poblarSelect(deps.distrito, [], "", "");

            if (id) {
                cargarUbigeo(`/Trabajadores/ObtenerProvincias?idDepartamento=${id}`, deps.provincia, "nombreProvincia", "id");
            }
        });

        deps.provincia.addEventListener("change", () => {
            const id = deps.provincia.value;
            poblarSelect(deps.distrito, [], "", "");

            if (id) {
                cargarUbigeo(`/Trabajadores/ObtenerDistritos?idProvincia=${id}`, deps.distrito, "nombreDistrito", "id");
            }
        });
    }

    // Inicializar listeners de cascada
    configurarUbigeoCascada(selects.crear);
    configurarUbigeoCascada(selects.editar);

    // Modal editar
    const modalEditar = document.getElementById("modalEditarTrabajador");
    modalEditar.addEventListener("show.bs.modal", function (event) {
        const button = event.relatedTarget;
        const deps = selects.editar;

        document.getElementById("editarId").value = button.getAttribute("data-id");
        document.getElementById("editarTipoDocumento").value = button.getAttribute("data-tipodocumento");
        document.getElementById("editarNumeroDocumento").value = button.getAttribute("data-numerodocumento");
        document.getElementById("editarNombres").value = button.getAttribute("data-nombres");
        document.getElementById("editarSexo").value = button.getAttribute("data-sexo");

        const departamentoId = button.getAttribute("data-iddepartamento");
        const provinciaId = button.getAttribute("data-idprovincia");
        const distritoId = button.getAttribute("data-iddistrito");

        deps.departamento.value = departamentoId;
        poblarSelect(deps.provincia, [], "", "");
        poblarSelect(deps.distrito, [], "", "");

        if (departamentoId) {
            cargarUbigeo(`/Trabajadores/ObtenerProvincias?idDepartamento=${departamentoId}`, deps.provincia, "nombreProvincia", "id", () => {
                deps.provincia.value = provinciaId;

                if (provinciaId) {
                    cargarUbigeo(`/Trabajadores/ObtenerDistritos?idProvincia=${provinciaId}`, deps.distrito, "nombreDistrito", "id", () => {
                        deps.distrito.value = distritoId;
                    });
                }
            });
        }
    });
});
