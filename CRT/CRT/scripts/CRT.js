$().ready(function () {
    $.ajax({
        url: "/ContentRequestTool.asmx/" + $(".btnDetalle").attr("data-method") + "Count",
        async: false,
        type: 'POST',        
        success: function (data) {
            $("#hdnLastId").val(data)
        }
    });


    $(".btnDetalle").click(function () {
        if ($("#languages option:selected").val() == "#") {
            alert("¡¡Debes seleccionar un idioma!!")
        }
        else {
            $("#btnDetalle").hide();
            $("#loading").show();
            $.ajax({
                url: "/ContentRequestTool.asmx/" + $(this).attr("data-method"),
                async: true,
                type: 'POST',
                data: {
                    language: $("#languages option:selected").val()
                },
                success: function (data) {
                    $("#loading").hide();
                    $("#btnDetalle").show();
                }
            });
        }       
    });
});
