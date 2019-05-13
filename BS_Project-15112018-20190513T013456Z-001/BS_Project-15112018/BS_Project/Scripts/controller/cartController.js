var cart = {
    init: function () {
        cart.regEvents();
    },
    regEvents: function () {
        //binding su kien
        //id bo dau thang, class dau cham, element khong can
        //off clear click
        //on binding su kien click cho funtion
        $('#btnContinue').off('click').on('click', function () {
            window.location.href = "/";
        });
        $("#btnUpdate").off('click').on('click', function () {
            var listBook = $('.txtQuantity');
            var cartList = [];
            $.each(listBook, function (i, item) {
                cartList.push({
                    Quantity: $(item).val(),
                    Books: {
                        BookID: $(item).data('id')
                    }
                });
            });

            $.ajax({
                url: '/CartItem/Update',
                data: { cartModel: JSON.stringify(cartList) },
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status) {
                        window.location.href = "/gio-hang";
                    } else {
                        $("#myFail #message").text(res.ErrorMessage);
                        $('#myFail').modal('show');
                    }
                }
            });
        });

        $("#btnDeleteAll").off('click').on('click', function () {
            $.ajax({
                url: '/CartItem/DeleteAll',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status) {
                        window.location.href = "/gio-hang";
                    }
                }
            });
        });

        $(".btn-delete").off('click').on('click', function (e) {
            //Bỏ qua # trong href
            e.preventDefault();

            //var quantity = $(this).closest('tr').find("input").val();
            //alert(quantity);
            $.ajax({
                data: {
                    id: $(this).data('id')
                },
                url: '/CartItem/DeleteByID',
                dataType: 'json',
                type: 'POST',
                success: function (res) {
                    if (res.status) {
                        $('#deleteSuccess').modal('show');
                        window.location.href = "/gio-hang";
                    }
                }
            });
        });

        $('#btnSubmit').off('click').on('click', function () {
            window.location.href = "/thanh-toan";
        });


    }
};
cart.init();