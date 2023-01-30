// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function precioSala() {
    let functionId = document.getElementById("FuncionId").value;
    
    let precioButacaTxt = document.getElementById("PrecioButacaTxt");
    let cantidadButacasTxt = document.getElementById("CantidadButacasTxt");
    let totalTxt = document.getElementById("TotalTxt");

    if (functionId != -1) {
        $.ajax({
            url: './PrecioSalaPorFuncion?idFuncion=' + functionId,
            type: 'GET',
            contentType: 'application/json;',
            success: function (valid) {
                if (valid) {
                    let cantidadButacas = document.getElementById("CantidadButacas").value;
                    console.log(cantidadButacas);
                    if (cantidadButacas == null || !cantidadButacas.trim().length) {
                        cantidadButacas = 0;
                    }
                    let precioSala = valid.precio;
                    let total = cantidadButacas * precioSala;


                    precioButacaTxt.textContent = precioSala;
                    cantidadButacasTxt.textContent = cantidadButacas;
                    totalTxt.textContent = total;

                } else {
                    //show that id is not valid 
                }
            }
        });
    } else {
        precioButacaTxt.textContent = 0;
        cantidadButacasTxt.textContent = 0;
        totalTxt.textContent = 0;
    }
    
}