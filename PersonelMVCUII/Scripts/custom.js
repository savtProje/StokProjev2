$(function () {
    $("#tblDepartmanlar").on("click", ".btnDepartmanSil", function () {
        var btn = $(this);
        bootbox.confirm("Departmanı Silmek İstediğinize emin misiniz", function (result) {
            if (result) {
                var id = btn.data("id");
                $.ajax({
                    type: "GET",
                    url: "/Departman/Sil/",
                    data: { id=id }
                    success: function () {
                        btn.parent().parent().remove();
                    }
                });
            }
        }
        )

    });
});









//function CheckDataTypeValid(dateElement) {
//    var value = $(dateElement).val();
//    if (value == "") {
//        $(dateElement).valid("false");
//    }
//    else {
//        $(dateElement).valid();
//    }
//}