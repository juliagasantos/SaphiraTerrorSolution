//java script puro
//document.getElementById('category');

//scripts do site usando jquery
//esta lista abaixo (evento) garante que o documento esta pronto (carregado)

$(document).ready(function () {
    //urlBase da API
    urlBase = "http://localhost:5274/api"

    
    console.log('Chamando a API');

    //consumindo a API usando AJAX
    $.ajax({
        type: "GET",
        url: urlBase + "/Genero", //endpoint
        type: "GET", //metodo http
        contentType: "application/json",

        //se der sucesso (200) cai aqui nesse bloco
        success: function (dados) {
            console.log('Deu sucesso!');

            // //percorrendo a lista de categorias
            //$.each(data, function (i, item) {
                //adicionando as categorias no select
                //$('#category').append($('<option>', {
                   // value: item.id,
                   // text: item.name
                //}));
           // });
        },

        //se der erro (400, 500) cai aqui nesse bloco
        error: function (error) {
            console.log('Deu erro!');
        }
    });


});