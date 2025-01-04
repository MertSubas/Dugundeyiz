let loginPartnerCallCount = 0; // Çağrı sayısını takip eden değişken
const MAX_CALLS_BEFORE_TEST = 5; // İnsan testi yapılmadan önceki maksimum çağrı sayısı
function ilanlarim(userID) {
    // Build the URL with the userID as a query parameter if needed
    let url = '/Hizmetler/ilanlarim';

    // If your backend requires the userID in the URL as a parameter
    url += `?userID=${userID}`;

    // Navigate to the URL
    window.location.href = url;
}

if (typeof inputElements === 'undefined') {
    const inputElements = ['user-username', 'user-password', 'partner-username', 'partner-password'];
    inputElements.forEach(inputId => {
        const input = document.getElementById(inputId);
        if (input) {
            input.addEventListener('focus', function () {
                this.previousElementSibling.classList.add('active-icon'); // Add class to icon
                this.parentElement.style.borderBottom = '1px solid #c7b38b';
                this.parentElement.style.boxShadow = '0 4px 15px #c7b38b';
                this.parentElement.style.borderRadius = '10px';
            });
            input.addEventListener('blur', function () {
                this.previousElementSibling.classList.remove('active-icon'); // Remove class from icon
                this.parentElement.style.borderBottom = '1px solid #ccc';
                this.parentElement.style.boxShadow = '0 0px 0px #fff';
            });
        }
    });
}






// JavaScript to control visibility of the tab content based on active tab
document.addEventListener('DOMContentLoaded', function () {
    var tabs = document.querySelectorAll('.nav-link');
    var tabContent = document.getElementById('loginTabContent');
    var lastActiveTab = null; // Son aktif sekmeyi izlemek için

    // Sekme içeriğini aktifliğe göre göster/gizle
    function checkActiveTab() {
        var activePane = document.querySelector('.tab-pane.active');
        if (activePane != null) {
            if (activePane) {
                tabContent.style.display = 'block';
            } else {
                tabContent.style.display = 'none';
            }
        }
    }

    document.addEventListener('click', function (event) {
        // LoginTabContent kontrolü
        var tabs = document.querySelectorAll('.nav-link');
        if (!event.target.classList.contains('nav-link')) {

            if (tabs.length > 0) {
                tabs.forEach(function (tab) {
                    if (tab.classList.contains('active')) {
                        var loginTabContent = document.getElementById('loginTabContent');
                        if (loginTabContent && loginTabContent.style.display !== 'none') {
                            // Eğer tıklanan yer loginTabContent veya içeriği değilse
                            if (!loginTabContent.contains(event.target)) {
                                // loginTabContent'i gizle
                                loginTabContent.style.display = 'none';
                                lastActiveTab = null; // Son aktif sekmeyi izlemek için

                                // nav-link elemanlarından active class'ını kaldır
                                var tabs = document.querySelectorAll('.nav-link');
                                if (tabs.length > 0) {
                                    tabs.forEach(function (tab) {
                                        if (tab.classList.contains('active')) {
                                            tab.classList.remove('active');
                                        }
                                    });
                                }
                                var tabz = document.querySelectorAll('.nav-item');
                                if (tabz.length > 0) {
                                    tabz.forEach(function (tab) {
                                        if (tab.classList.contains('active')) {
                                            tab.classList.remove('active');
                                        }
                                    });
                                }
                            }
                        }

                    }
                });
            }
        }

    });
    // Butona tıklayınca modalı aç


    // Her bir sekmeye tıklama olayları ekle
    tabs.forEach(function (tab) {
        tab.addEventListener('click', function (e) {
            e.preventDefault(); // Varsayılan sekme davranışını engelle

            // Eğer tıklanan sekme zaten aktifse kapat
            if (lastActiveTab === e.target) {
                tab.classList.remove('active'); // Sekme aktifliğini kaldır
                var paneId = tab.getAttribute('href'); // İlgili sekme içeriğini al
                document.querySelector(paneId).classList.remove('active', 'show'); // İçeriği kapat
                lastActiveTab = null; // Son aktif sekmeyi sıfırla
                tabContent.style.display = 'none'; // İçeriği gizle
                var parentDiv = tab.parentElement; // Bir üst div'e ulaş
                if (parentDiv) {
                    parentDiv.classList.remove('active'); // Üst div'deki 'active' sınıfını kaldır
                    event.preventDefault();
                    e.stopPropagation(); // Olayın yayılmasını engelle

                }
            } else {
                // Eğer farklı bir sekmeye tıklanırsa veya aynı sekmeye tekrar açılmak istenirse
                tabs.forEach(function (t) {
                    t.classList.remove('active'); // Tüm sekmelerde aktiflik kaldırılır
                    var paneId = t.getAttribute('href');
                    document.querySelector(paneId).classList.remove('active', 'show'); // Tüm içerikler gizlenir
                });
                if (lastActiveTab != e.target) {
                    tab.classList.add('active'); // Tıklanan sekme aktif edilir
                    var paneId = tab.getAttribute('href');
                    document.querySelector(paneId).classList.add('active', 'show'); // İlgili içerik gösterilir
                    lastActiveTab = e.target; // Son aktif sekmeyi güncelle
                    tabContent.style.display = 'block'; // İçeriği göster
                }
            }
        });
    });

    // Sayfa yüklendiğinde ilk kontrol
    checkActiveTab();
});


async function logoutUser() {

    try {
        debugger;
        const response = await fetch('/Account/LogOut', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify() // Veriyi JSON formatında gönder
        });

        if (response.ok) {
            // const result = await response.json(); // İsteğin sonucunu al
            // document.getElementById('message').innerText = 'Kayıt başarılı!';
            // Gerekirse yönlendirme yap
            // window.location.href = '/Home/Index';
            window.location.href = '/';

        } else {
            const errorResponse = await response.json();
            document.getElementById('message').innerText = errorResponse.message || 'Kayıt başarısız!';
        }
    } catch (error) {
        console.error('Hata:', error);
        document.getElementById('message').innerText = 'Bir hata oluştu!';
    }
}
document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.custom-dropdown-login-btn').forEach(button => {
        button.addEventListener('click', function () {
            const dropdownMenu = this.nextElementSibling; // nextElementSibling ile <ul> öğesini alıyoruz

            // Menü görünürse gizle
            if (dropdownMenu.classList.contains('d-none')) {
                dropdownMenu.classList.remove('d-none'); // Menüyü gizle
            } else {
                dropdownMenu.classList.add('d-none'); // Menüyü göster
            }
        });
    });

    document.querySelectorAll('.custom-dropdown-login-item').forEach(item => {
        item.addEventListener('click', function () {
            const dropdownMenu = this.closest('.custom-dropdown-login-menu');

            // Menü kapanacaksa, .show sınıfını kaldırıyoruz
            dropdownMenu.classList.add('d-none');
        });
    });
    var partnerModal = new bootstrap.Modal(document.getElementById('partnerMobilLogin'));

    document.querySelectorAll('.partner-login-dropdown').forEach(item => {
        item.addEventListener('click', function () {

            partnerModal.show();
        });
    });
    var openpartnerModal2 = document.getElementById("openPartnerModal2");
    openpartnerModal2.addEventListener("click", function (e) {
        partnerModal.hide();

        // Modalı kapat
    });


    var userModal = new bootstrap.Modal(document.getElementById('userMobilLogin'));

    document.querySelectorAll('.user-login-dropdown').forEach(item => {
        item.addEventListener('click', function () {

            userModal.show();
        });
    });

    var openUserModal2 = document.getElementById("openUserModal2");
    openUserModal2.addEventListener("click", function (e) {
        userModal.hide();

        // Modalı kapat
    });


    // Button elementine tıklama olayı ekle
    const partnerLoginButton = document.getElementById('partnerLoginMobil');

    partnerLoginButton.addEventListener('click', function () {
        // Input değerlerini al
        const username = document.getElementById('partnerMobil-username').value.trim();
        const password = document.getElementById('partnerMobil-password').value.trim();

        // Verileri konsola yazdır veya işleme devam et
        console.log('Kullanıcı Adı:', username);
        console.log('Şifre:', password);
        const mobil = true;
        // Eğer verilerle işlem yapılacaksa burada ekleyebilirsiniz
        if (username && password) {
            // Örnek işlem: AJAX ile sunucuya gönderme
            LoginPartner(username, password,mobil);
        } else {
            // Eksik bilgi uyarısı
        }
    });
    const userLoginButton = document.getElementById('userLoginMobil');

    userLoginButton.addEventListener('click', function () {
        // Input değerlerini al
        const username = document.getElementById('userMobil-username').value.trim();
        const password = document.getElementById('userMobil-password').value.trim();
        const mobil = true;
        // Verileri konsola yazdır veya işleme devam et
        console.log('Kullanıcı Adı:', username);
        console.log('Şifre:', password);

        // Eğer verilerle işlem yapılacaksa burada ekleyebilirsiniz
        if (username && password) {
            // Örnek işlem: AJAX ile sunucuya gönderme
            LoginUser(username, password,mobil);
        } else {
            // Eksik bilgi uyarısı

        }
    });
});


// Modal dışına tıklanarak kapanma Bootstrap tarafından otomatik sağlanır


function validateAndLogin(type) {
    // Form alanlarını belirle
    var username = document.getElementById(type + '-username').value;
    var password = document.getElementById(type + '-password').value;

    // Hata mesajı alanlarını temizle
    clearErrors(type);

    var isValid = true;
    var messageErrorPartner;
    // Boş alan kontrolü ve hata mesajı ekleme
    if (password === '' || username === '') {
        if (username === '') {
            messageErrorPartner = 'Kullanıcı Adınızı Giriniz.', 'Kullanıcı Adı boş geçilemez.';
        }
        if (password === '') {
            messageErrorPartner = 'Şifrenizi Giriniz.', 'Şifre boş geçilemez.';
        }
        if (password === '' && username === '') {
            messageErrorPartner = 'Kullanıcı Adınızı ve Şifrenizi Giriniz.';
        }

        showGeneralError(type, messageErrorPartner);
        isValid = false;
    }

    // Eğer tüm alanlar doluysa, giriş işlemini deneriz
    if (isValid) {
        const mobil = false;
        if (type === 'user') {
            // Örnek olarak: Hatalı giriş simülasyonu
            // if (username !== "a" || password !== "a") {
            //     showGeneralError(type, 'Hatalı kullanıcı adı veya şifre.');
            // } else {
            LoginUser(username, password,mobil);
            // }
        } else if (type === 'partner') {
            // Örnek olarak: Hatalı giriş simülasyonu
            // if (username !== "correctPartner" || password !== "correctPartnerPassword") {
            //     showGeneralError(type, 'Hatalı kullanıcı adı veya şifre.');
            // } else {
            LoginPartner(username, password,mobil);
            // }
        }
    }
}

// Genel hata mesajını gösterme fonksiyonu
function showGeneralError(type, message) {
    var errorElement = document.getElementById(type + '-login-error');
    errorElement.innerHTML = message;
    errorElement.classList.remove('d-none'); // Hata mesajını görünür yap
}

// Hata mesajı gösterme fonksiyonu
function showError(inputId, message) {
    var errorElement = document.getElementById(inputId + '-error');
    errorElement.innerHTML = message;
    errorElement.style.display = 'block';
    document.getElementById(inputId).classList.add('is-invalid'); // Bootstrap invalid sınıfı
}

// Hata mesajlarını temizleme fonksiyonu
function clearErrors(type) {
    var errorFields = ['username', 'password'];
    errorFields.forEach(function (field) {
        var inputId = type + '-' + field;
        document.getElementById(inputId + '-error').innerHTML = '';
        document.getElementById(inputId).classList.remove('is-invalid');
    });

    // Genel hata mesajını temizle
    var generalErrorElement = document.getElementById(type + '-login-error');
    generalErrorElement.classList.add('d-none'); // Genel hata mesajını gizle
    generalErrorElement.innerHTML = '';
}

// LoginUser fonksiyonu
async function LoginUser(username, password,mobil) {

    const data = {
        userName: username,
        password: password
    }
    try {
        const response = await fetch('/Account/LoginUser', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data) // Veriyi JSON formatında gönder
        });

        if (response.ok) { // HTTP durum kodu 200-299 aralığında mı?
            const jsonData = await response.json(); // JSON verisini ayrıştır
            if (jsonData.type == 2) { // Type özelliğini kontrol et
                showGeneralError('user', jsonData.message);
                const errorDiv = document.getElementById("userMobilLoginError");
                if (!errorDiv) {
                    console.error("Element with ID 'userMobilLoginError' not found.");
                } else {
                    errorDiv.classList.remove("d-none");  // CSS'e öncelik vermek içinmainMemberSignUpError
                    errorDiv.innerText = jsonData.message;
                }
            } else {
                const htmlContent = await response.text(); // Yanıtı HTML olarak işleme userMobilLoginError
                document.querySelector("#login-data-container").innerHTML = htmlContent;
            }
        } else {
            showGeneralError('user', "Bir hata Oluştu Lütfen Tekrar Deneyin");
        }

    } catch (error) {
        console.error('Hata:', error);
        document.getElementById('message').innerText = 'Bir hata oluştu!';
    }
}
// LoginPartner fonksiyonu
async function LoginPartner(username, password,mobil) {
    // Giriş işlemlerini yapabilirsiniz
    loginPartnerCallCount++; // Çağrı sayısını artır
    if (mobil == false) {
        if (loginPartnerCallCount > MAX_CALLS_BEFORE_TEST) {
            const captchaElement = document.getElementById("partner-captcha");
            captchaElement.classList.remove("d-none"); // CAPTCHA alanını göster
            const captchaResponse = grecaptcha.getResponse();

            // CAPTCHA doğrulaması yapılmamışsa hata göster ve işlemi durdur
            if (!captchaResponse) {
                document.getElementById("partner-login-error").classList.remove("d-none");
                document.getElementById("partner-login-error").innerText =
                    "Lütfen Doğrulamayı Geçip Tekrar Giriş Yapın";
                return; // Login işlemini durdur
            }
        }
    }

    const data = {
        userName: username,
        password: password
    };

    try {
        const response = await fetch('/Account/LoginPartner', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(data) // Veriyi JSON formatında gönder
        });

        if (response.ok) { // HTTP durum kodu 200-299 aralığında mı?
            const contentType = response.headers.get('Content-Type'); // Yanıtın içeriğini kontrol et
            if (contentType && contentType.includes('text/html')) {
                const htmlContent = await response.text(); // HTML içeriği al
                document.querySelector("#login-data-container").innerHTML = htmlContent; // HTML'i hedef div'e yerleştir
            } else if (contentType && contentType.includes('application/json')) {
                const jsonData = await response.json(); // JSON verisini ayrıştır
                if (jsonData.type === 2) {
                    showGeneralError('partner', jsonData.message);
                    const errorDiv = document.getElementById("partnerMobilLoginError");
                    if (!errorDiv) {
                        console.error("Element with ID 'mainPartnerSignUpError' not found.");
                    } else {
                        errorDiv.classList.remove("d-none");  // CSS'e öncelik vermek içinmainMemberSignUpError
                        errorDiv.innerText = jsonData.message;
                    }
                } else {
                    showGeneralError('user', "Beklenmeyen JSON formatı.");
                }
            } else {
                showGeneralError('user', "Beklenmeyen bir içerik türü alındı.");
            }
        } else {
            showGeneralError('user', "Bir hata Oluştu Lütfen Tekrar Deneyin");
        }
    } catch (error) {
        console.error('Hata:', error);
        document.getElementById('message').innerText = 'Bir hata oluştu!';

    }
}

document.querySelectorAll('profile-navbar-item').forEach(item => {
    item.addEventListener('mouseover',
        function () {
            this.querySelector('.dropdown-menu').style.display = 'block';

        });
    item.addEventListener('mouseout',
        function () {
            this.querySelector('.dropdown-menu').style.display = 'none';

        });

})



$('#partnerSignUpSubmit').click(function (e) {
    e.preventDefault();
    let isValid = true;

    // Input ve textarea alanlarını kontrol et
    $('.modal-content .row .col-md-6 input[required], .modal-content .row .col-md-6 textarea[required]').each(function () {
        const $errorSpan = $(this).next('.error-message');
        if (!$(this).val()) {
            $errorSpan.css('display', 'block'); // Hata mesajını göster
            $(this).css('border-color', 'red'); // Border kırmızı yap
            isValid = false;
        } else {
            $errorSpan.css('display', 'none'); // Hata mesajını gizle
            $(this).css('border-color', ''); // Border eski haline dön
        }
    });
    // Telefon doğrulama
    const phoneInput = $('#partnerPhone');
    const phoneRegex = /^\(\d{3}\)-\d{3}-\d{2}-\d{2}$/;
    if (!phoneRegex.test(phoneInput.val())) {
        const $errorSpan = phoneInput.next('.error-message');
        $errorSpan.text('Lütfen telefon numaranızı 10 hane olarak giriniz.').show();
        phoneInput.css('border-color', 'red');
        isValid = false;
    } else {
        phoneInput.next('.error-message').text('').hide();
        phoneInput.css('border-color', '');
    }
    // Email doğrulama
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    const emailInput = $('#partnerEmail');
    if (!emailRegex.test(emailInput.val())) {
        const $errorSpan = emailInput.next('.error-message');
        $errorSpan.text('Lütfen emailinizi kontrol edin. Örnek yazılım şekli: "ornek@mail.com"').show();
        emailInput.css('border-color', 'red');
        isValid = false;
    } else {
        emailInput.next('.error-message').text('').hide();
        emailInput.css('border-color', '');
    }
    // Dropdown kontrolü (Şehir)
    if (!$('#cityDropdown').text().trim() || $('#cityDropdown').text().trim() === 'Şehir Seç') {
        const $errorSpan = $('#cityDropdown').closest('.col-md-6').find('.error-message');
        $errorSpan.text('Bu alan zorunludur.').show();
        isValid = false;
    } else {
        $('#cityDropdown').closest('.col-md-6').find('.error-message').text('').hide();
    }

    // Dropdown kontrolü (İlçe)
    if (!$('#districtDropdown').text().trim() || $('#districtDropdown').text().trim() === 'İlçe Seç') {
        const $errorSpan = $('#districtDropdown').closest('.col-md-6').find('.error-message');
        $errorSpan.text('Bu alan zorunludur.').show();
        isValid = false;
    } else {
        $('#districtDropdown').closest('.col-md-6').find('.error-message').text('').hide();
    }

    // Şifre kontrolü
    if ($('#partnerPassword').val() !== $('#partnerPasswordCheck').val()) {
        const $errorSpan = $('#partnerPasswordCheck').next('.error-message');
        $errorSpan.text('Şifreler uyuşmuyor.').show();
        isValid = false;
    } else {
        $('#partnerPasswordCheck').next('.error-message').text('').hide();
    }

    if (isValid) {
        const data = {
            username: $('#partnerUsername').val(),
            name: $('#partnerName').val(),
            surname: $('#partnerSurname').val(),
            companyName: $('#partnerCompanyName').val(),
            address: $('#partnerAddress').val(),
            city: $('#cityDropdown').text(),
            district: $('#districtDropdown').text(),
            email: $('#partnerEmail').val(),
            phone: phoneInput.val().replace(/\D/g, ''), // Sadece rakamları gönder
            password: $('#partnerPassword').val(),
        };

        $.ajax({
            url: "/Account/RegisterPartner",
            type: "POST",
            data: JSON.stringify(data),
            contentType: "application/json",
            success: function (response) {
                if (response.type == 2) {
                    const errorDiv = document.getElementById("mainPartnerSignUpError");
                    if (!errorDiv) {
                        console.error("Element with ID 'mainPartnerSignUpError' not found.");
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
        });
    }
});

