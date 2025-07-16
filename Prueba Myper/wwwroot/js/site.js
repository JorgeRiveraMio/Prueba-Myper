
    document.addEventListener("DOMContentLoaded", function () {
        const departamentoSelect = document.getElementById("departamento");
        const provinciaSelect = document.getElementById("provincia");
        const distritoSelect = document.getElementById("distrito");

        departamentoSelect.addEventListener("change", function () {
            const departamentoId = this.value;
            provinciaSelect.innerHTML = "<option value=''>-- Selecciona --</option>";
            distritoSelect.innerHTML = "<option value=''>-- Selecciona --</option>";

            if (departamentoId) {
                fetch(`/Trabajadores/ObtenerProvincias?idDepartamento=${departamentoId}`)
                    .then(response => response.json())
                    .then(data => {
                        data.forEach(p => {
                            const option = document.createElement("option");
                            option.value = p.id;
                            option.textContent = p.nombreProvincia;
                            provinciaSelect.appendChild(option);
                        });
                    });
            }
        });

        provinciaSelect.addEventListener("change", function () {
            const provinciaId = this.value;
            distritoSelect.innerHTML = "<option value=''>-- Selecciona --</option>";

            if (provinciaId) {
                fetch(`/Trabajadores/ObtenerDistritos?idProvincia=${provinciaId}`)
                    .then(response => response.json())
                    .then(data => {
                        data.forEach(d => {
                            const option = document.createElement("option");
                            option.value = d.id;
                            option.textContent = d.nombreDistrito;
                            distritoSelect.appendChild(option);
                        });
                    });
            }
        });
    });


    const modalEditar = document.getElementById("modalEditarTrabajador");

    modalEditar.addEventListener("show.bs.modal", function (event) {
        const button = event.relatedTarget;

        // Cargar valores al abrir el modal
        document.getElementById("editarId").value = button.getAttribute("data-id");
        document.getElementById("editarTipoDocumento").value = button.getAttribute("data-tipodocumento");
        document.getElementById("editarNumeroDocumento").value = button.getAttribute("data-numerodocumento");
        document.getElementById("editarNombres").value = button.getAttribute("data-nombres");
        document.getElementById("editarSexo").value = button.getAttribute("data-sexo");

        const departamentoId = button.getAttribute("data-iddepartamento");
        const provinciaId = button.getAttribute("data-idprovincia");
        const distritoId = button.getAttribute("data-iddistrito");

        const departamentoSelect = document.getElementById("editarDepartamento");
        const provinciaSelect = document.getElementById("editarProvincia");
        const distritoSelect = document.getElementById("editarDistrito");

        departamentoSelect.value = departamentoId;

        // Limpiar combos dependientes
        provinciaSelect.innerHTML = "<option value=''>-- Selecciona --</option>";
        distritoSelect.innerHTML = "<option value=''>-- Selecciona --</option>";

        if (departamentoId) {
            fetch(`/Trabajadores/ObtenerProvincias?idDepartamento=${departamentoId}`)
                .then(res => res.json())
                .then(provincias => {
                    provincias.forEach(p => {
                        const option = document.createElement("option");
                        option.value = p.id;
                        option.textContent = p.nombreProvincia;
                        provinciaSelect.appendChild(option);
                    });
                    provinciaSelect.value = provinciaId;

                    if (provinciaId) {
                        fetch(`/Trabajadores/ObtenerDistritos?idProvincia=${provinciaId}`)
                            .then(res => res.json())
                            .then(distritos => {
                                distritos.forEach(d => {
                                    const option = document.createElement("option");
                                    option.value = d.id;
                                    option.textContent = d.nombreDistrito;
                                    distritoSelect.appendChild(option);
                                });
                                distritoSelect.value = distritoId;
                            });
                    }
                });
        }
    });

    // Listeners en cascada (igual que el de crear)
    document.getElementById("editarDepartamento").addEventListener("change", function () {
        const depId = this.value;
        const provinciaSelect = document.getElementById("editarProvincia");
        const distritoSelect = document.getElementById("editarDistrito");
        provinciaSelect.innerHTML = "<option value=''>-- Selecciona --</option>";
        distritoSelect.innerHTML = "<option value=''>-- Selecciona --</option>";

        if (depId) {
            fetch(`/Trabajadores/ObtenerProvincias?idDepartamento=${depId}`)
                .then(response => response.json())
                .then(data => {
                    data.forEach(p => {
                        const option = document.createElement("option");
                        option.value = p.id;
                        option.textContent = p.nombreProvincia;
                        provinciaSelect.appendChild(option);
                    });
                });
        }
    });

    document.getElementById("editarProvincia").addEventListener("change", function () {
        const provId = this.value;
        const distritoSelect = document.getElementById("editarDistrito");
        distritoSelect.innerHTML = "<option value=''>-- Selecciona --</option>";

        if (provId) {
            fetch(`/Trabajadores/ObtenerDistritos?idProvincia=${provId}`)
                .then(response => response.json())
                .then(data => {
                    data.forEach(d => {
                        const option = document.createElement("option");
                        option.value = d.id;
                        option.textContent = d.nombreDistrito;
                        distritoSelect.appendChild(option);
                    });
                });
        }
    });

