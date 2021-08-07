function AjaxModal() {
  $(document).ready(function () {
    $(function () {
      $.ajaxSetup({ cache: false });

      $("a[data-modal]").on("click",
        function (e) {
          $('#myModalContent').load(this.href,
            function () {
              $.('#myModal').modal({
                keyboard: true
              }, 'show');
              bindForm(this);
            });
          return false;
        })
    });

    function bindForm(dialog) {
      $('form', dialog).submit(function () {
        $.ajax({
          url: this.action,
          type: this.method,
          data: $(this).serialize(),
          success: function (result) {
            if (result.success) {
              $('#myModal').modal('hide');
              $('#AddressTarget').load(result.url);
            }
            else {
              $('#myModalContent').html(result);
              bindForm(dialog);
            }
          }
        });
        return false;
      });
    }
  });
}

function SearchZipCode() {
  $(document).ready(function () {
    function cleanZipCodeForm() {
      //Clean values of zipcode's form
      $("#Address_PublicPlace").val("");
      $("#Address_District").val("");
      $("#Address_City").val("");
      $("#Address_State").val("");
    }

    $("Address_Zipcode").blur(function () {
      const zipcode = $(this).val().replace(/\D/g, '');

      if (zipcode != "") {
        const validZipcode = /^[0-9]{8}$/;

        if (validZipcode.test(zipcode)) {
          $("#Address_PublicPlace").val("...");
          $("#Address_District").val("...");
          $("#Address_City").val("...");
          $("#Address_State").val("...");

          $.getJSON("https://viacep.com.br/ws/" + zipcode + "/json/?callback=?",
            function (data) {
              if (!("erro" in data)) {
                $("#Address_PublicPlace").val(data.logradouro);
                $("#Address_District").val(data.bairro);
                $("#Address_City").val(data.localidade);
                $("#Address_State").val(data.uf);
              }
              else {
                cleanZipCodeForm();
                alert("CEP não encontrado!");
              }
            });
        }
        else {
          cleanZipCodeForm();
          alert("Formato de CEP inválido!");
        }
      }
      else {
        cleanZipCodeForm();
      }
    });
  })
}

$(document).ready(function () {
  $("#boxMessage").fadeOut(2500);
})