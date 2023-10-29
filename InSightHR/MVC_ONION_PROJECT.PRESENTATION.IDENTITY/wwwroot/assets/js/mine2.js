
let isSubmitting = false;

// Tüm form elementlerini seçin ve submit olayını dinleyin
const forms = document.querySelectorAll("form");
forms.forEach(function (form) {
    form.addEventListener("submit", function (e) {
        if (isSubmitting) {
            e.preventDefault();
        } else {
            isSubmitting = true;
            // İşlem devam ediyor...
        }
    });
});
