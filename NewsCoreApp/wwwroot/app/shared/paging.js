var paging = {
    wrapPaging: function (recordCount, callBack, changePageSize) {
        var totalsize = Math.ceil(recordCount / lkd.configs.pageSize);
        //Unbind pagination if it existed or click change pagesize
        if ($('#paginationUL a').length === 0 || changePageSize === true) {
            $('#paginationUL').empty();
            $('#paginationUL').removeData("twbs-pagination");
            $('#paginationUL').unbind("page");
        }
        //Bind Pagination Event
        $('#paginationUL').twbsPagination({
            totalPages: totalsize,
            visiblePages: 7,
            first: 'Đầu',
            prev: 'Trước',
            next: 'Tiếp',
            last: 'Cuối',
            onPageClick: function (event, p) {
                lkd.configs.pageIndex = p;
                setTimeout(callBack(), 200);
            }
        });
    }
}

var pagingSec = document.getElementById('pagingSection');
var strHtml = "";
if (pagingSec !== null) {
    strHtml += "<select id=\"ddlShowPage\">";
    strHtml += "<option value=\"10\" selected=\"selected\">10</option>";
    strHtml += "<option value=\"20\">20</option>";
    strHtml += "<option value=\"30\">30</option>";
    strHtml += "<option value=\"50\">50</option>";
    strHtml += "</select>";
    strHtml += "<span class=\"item-per-page\">bản ghi/trang.</span>";
    strHtml += "Tổng số bản ghi: <strong id=\"lblTotalRecords\"></strong>";
    strHtml += "<ul id=\"paginationUL\" class=\"pagination no-margin pull-right\"></ul>";

    pagingSec.innerHTML = strHtml;
}