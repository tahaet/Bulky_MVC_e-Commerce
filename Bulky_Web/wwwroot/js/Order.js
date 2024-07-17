let dataTable;
$("document").ready(() => {

    const url = window.location.search;
    let filter;
    if (url.includes("inprocess")) {
        filter = "inprocess";
    }
    if (url.includes("completed")) {
        filter = "completed";
    }
    if (url.includes("pending")) {
        filter = "pending";
    }
    if (url.includes("approved")) {
        filter = "approved";
    }
    if (url.includes("all")) {
        filter = "all";
    }
    loadDataTable(filter);
});
function loadDataTable(filter) {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/order/getall?status=' + filter },
        "columns": [
            { "data": "id", "width": "5%" },
            { "data": "name", "width": "20%" },
            { "data": "phoneNumber", "width": "20%" },
            { "data": "applicationUser.email", "width": "20%" },
            { "data": "orderStatus", "width": "10%" },
            { "data": "orderTotal", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                                 <a href="/admin/order/details?orderid=${data}" class="btn btn-primary mx-2">
                                      <i class="bi bi-pencil-square"></i>
                                 </a>
                            </div>`
                },
                "width": "10%"
            }
        ]
    });
}
