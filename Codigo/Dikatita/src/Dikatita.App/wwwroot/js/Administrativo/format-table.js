$(document).ready(function () {
    function initializeDataTable() {
        if ($.fn.DataTable.isDataTable('#table-pedidos')) {
            $('#table-pedidos').DataTable().destroy();
        }

        $('#table-pedidos').DataTable({
            "paging": true,
            "searching": true,
            "ordering": true,
            "info": true,
            "lengthMenu": [5, 10, 25, 50],
            "order": [[3, 'desc']],
            "language": {
                "search": "Buscar:",
                "lengthMenu": "Mostrar _MENU_ registros por página",
                "info": "Mostrando _START_ a _END_ de _TOTAL_ registros",
                "infoEmpty": "Nenhum registro encontrado",
                "paginate": {
                    "first": "Primeiro",
                    "last": "Último",
                    "next": "Próximo",
                    "previous": "Anterior"
                }
            }
        });
    }

    initializeDataTable();

    $(document).on('ajaxComplete', function () {
        initializeDataTable();
    });
});
