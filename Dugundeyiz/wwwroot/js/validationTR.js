// Validation on form submission
document.addEventListener('DOMContentLoaded', function () {

    document.getElementById('customerSignUpSubmit').addEventListener('click', function (e) {
        e.preventDefault(); // Prevent form submission for now
        let isValid = true;
        console.log("ı ş ğ i g s")
        // Clear previous error messages
        document.querySelectorAll('.error-message').forEach(function (errorMsg) {
            errorMsg.style.display = 'none';
        });

        // Username validation
        const username = document.getElementById('customerUsername');
        if (username.value.trim() === "") {
            showError(username, "Kullanıcı adı zorunludur.");
            isValid = false;
        }

        // Name validation
        const name = document.getElementById('customerName');
        if (name.innerText != null) {
            if (name.value.trim() === "") {
                showError(name, "Ad soyad zorunludur.");
                isValid = false;
            }
        }
        else {
            showError(name, "Ad soyad zorunludur.");
            isValid = false;
        }

        // Email validation
        const email = document.getElementById('customerEmail');
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email.value)) {
            showError(email, "Geçerli bir email adresi girin.");
            isValid = false;
        }

        // Phone validation
        const phoneInput = $('#customerPhone');
        const phoneRegex = /^\(\d{3}\)-\d{3}-\d{2}-\d{2}$/;
        if (!phoneRegex.test(phoneInput.val())) {
            const $errorSpan = phoneInput.next('.error-message');
            showError(phoneInput, "Lütfen telefon numaranızı 10 hane olarak giriniz.");

            //$errorSpan.text('Lütfen telefon numaranızı 10 hane olarak giriniz.').show();
            //phoneInput.css('border-color', 'red');
            isValid = false;
        } else {
            phoneInput.next('.error-message').text('').hide();
            phoneInput.css('border-color', '');
        }
        // Password validation
        const password = document.getElementById('customerPassword');
        if (password.value.length < 6) {
            showError(password, "Şifre en az 6 karakter olmalıdır.");
            isValid = false;
        }

        // Password confirmation validation
        const passwordCheck = document.getElementById('customerPasswordCheck');
        if (passwordCheck.value !== password.value) {
            showError(passwordCheck, "Şifreler uyuşmuyor.");
            isValid = false;
        }

        // If form is valid, proceed with form submission (here, just alert)
        if (isValid) {
            const data = {
                username: $('#customerUsername').val(),
                name: $('#customerName').val(),
                //surname: $('#partnerSurname').val(),
                //companyName: $('#partnerCompanyName').val(),
                //address: $('#partnerAddress').val(),
                //city: $('#cityDropdown').text(),
                //district: $('#districtDropdown').text(),
                email: $('#customerEmail').val(),
                phone: phoneInput.val().replace(/\D/g, ''), // Sadece rakamları gönder
                password: $('#customerPassword').val(),
            };
            $.ajax({
                url: "/Account/RegisterMember",
                type: "POST",
                data: JSON.stringify(data),
                contentType: "application/json",
                success: function (response) {
                    if (response.type == 2) {
                        const errorDiv = document.getElementById("mainMemberSignUpError");
                        if (!errorDiv) {
                            console.error("Element with ID 'mainMemberSignUpError' not found.");
                        } else {
                            errorDiv.classList.remove("d-none");  // CSS'e öncelik vermek içinmainMemberSignUpError
                            errorDiv.innerText = response.message;
                        }


                    }
                    else {
                        alert('Başarıyla kayıt oldunuz! Lütfen Giriş Yapın');
                        window.location.reload()
                    }
                },
                error: function (err) {
                    alert('Bir hata oluştu. Lütfen tekrar deneyin.');
                }
            });            // Form submission logic can go here
        }
    });

    // Show error function
    function showError(inputElement, message) {
        const errorElement = inputElement.parentNode.querySelector('.error-message');
        errorElement.innerHTML = message;
        errorElement.style.display = 'block';
        errorElement.css('border-color', 'red');
    }

});
