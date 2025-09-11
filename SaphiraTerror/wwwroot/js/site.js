//java script puro
//document.getElementById('category');

//scripts do site usando jquery
//esta lista abaixo (evento) garante que o documento esta pronto (carregado)

$(document).ready(function () {
    //urlBase da API
    const urlBase = "http://localhost:5118/api"

    
    console.log('Chamando a API');

    //consumindo a API usando AJAX
    $.ajax({
        url: urlBase + "/Genero", //endpoint
        type: "GET", //metodo http
        contentType: "application/json",

        //se der sucesso (200) cai aqui nesse bloco
        success: function (dados) {

            //selecione a div de generos
            const divGeneros = $('#genre');

            //percorre a lista de generos, criando o botao e add na div
            dados.forEach(genero => {
                //crie um botao para cada genero
                const btn = `<button class="btn btn-outline-light me-2 filter-btn" data-genero="${genero.idGenero}">${genero.descricao}</button>`;
                //adicione o botao na div de generos
                divGeneros.append(btn);
            });


            //botao de genero
            //<button class="btn btn-outline-light me-2 filter-btn" data-genero="todos">Todos</button>


            console.log('Deu sucesso!');
            console.log(dados);
            console.log(dados[1]);
            console.log(dados[1].descricao);
        },

        //se der erro (400, 500) cai aqui nesse bloco
        error: function (erro) {
            console.log('Deu erro!');
        }
    });
});