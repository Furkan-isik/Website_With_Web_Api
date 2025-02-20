$(function () {

    function ShowMessage(message)
    {
        var mesaj = $('.stateMessage');
        if (mesaj.text().trim() !== "") {
            mesaj.text("");
            setTimeout(function () {
                mesaj.text(message);
            }, 200);
        } else {
            mesaj.text(message);
        }
    }
    $("#registerButton").on("click", function () {

        var Name = $("input[name='Name']").val();
        var Username = $("input[name='Username']").val();
        var Email = $("input[name='Email']").val();
        var Password = $("input[name='Password']").val();
        var ConfirmPassword = $("input[name='ConfirmPassword']").val();


        if (!Name || !Username || !Email || !Password || !ConfirmPassword) {
            ShowMessage("Tüm alanları doldurunuz.");
            return;
        }

        if (Password !== ConfirmPassword) {
            ShowMessage("Şifreler eşleşmiyor.");
            return;
        }

        var userData = {
            Name: Name,
            UserName: Username,
            Email: Email,
            Password: Password
        };

        $.ajax({

            url:'http://localhost:8092/api/send/register',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(userData),
            success: function (response) {
                ShowMessage(response);

               // setTimeout(function () {
                    window.location.href = "loginPage.html";
               // },200);
            },
            error: function (xhr, status, error) {
                var hata = xhr.responseJSON ? xhr.responseJSON : { Message: "Hata oluştu!" };
                console.log(hata);
                ShowMessage('Kayıt oluşturulamadı: ' + hata.Message);
            }
        });
    });

    $("#loginButton").on("click", function () {

        var Username = $("input[name='Username']").val();
        var Password = $("input[name='Password']").val();

        if (!Username || !Password) {
            ShowMessage("Tüm alanları doldurun.")
            return;
        }

        var loginData = {
            UserName: Username,
            Password: Password
        };

        $.ajax({

            url: 'http://localhost:8092/api/send/login',
            type: 'POST',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(loginData),
            success: function (response) {
                ShowMessage(response);
                //setTimeout(function () {
                //    window.location.href = "Index.html";
                //}, 2000);
            },
            error: function (xhr, status, error) {
                var hata = xhr.responseJSON ? xhr.responseJSON : { Message: "Hata oluştu!" };
                console.log(hata);
                ShowMessage(hata.Message);
            }
        });
    }); 
});