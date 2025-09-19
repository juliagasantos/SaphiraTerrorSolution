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


    $.ajax({
        url: urlBase + "/Filme", //endpoint
        type: "GET", //metodo http
        contentType: "application/json",

        success: function (dados) {

            //selecione a div
            const divFilmes = $('#filmes-container');

            dados.forEach(filme => {

                corBagde = "success";
                if (filme.classificacao == "+18") {
                    corBagde = "danger";
                } else if (filme.classificacao == "Kids") {
                    corBagde = "primary";
                }

                const card = `<div class="col">
                <div class="card bg-dark border-secondary h-100">
                    <img src="${filme.urlImagem}" class="card-img-top" alt="${filme.titulo}" style="height: 250px; object-fit: cover;">
                    <div class="card-body">
                        <h5 class="card-title">${filme.titulo}</h5>
                        <p class="card-text text-muted">${filme.genero}</p>
                        <p class="card-text small text-muted">${filme.produtora}</p>
                        <span class="badge bg-${corBagde}">${filme.classificacao}</span>
                    </div>
                </div>
            </div>`;

                divFilmes.append(card);
            });
        },
        error: function (erro) {
            console.log('Erro ao carregar filmes!');
        }
    });

    
    //ap clicar em um dos generos (elemento button dentro da div id genre)
    $(document).on('click', '#genre button', function () {
        //pegar o valor do atributo 'data-genero' do botão clicado
        const generoEscolhido = $(this).data('genero');
        console.log('Gênero: ', generoEscolhido);

        //chamar a função buscarFilmes passando o genero escolhido
        BuscarFilmes(generoEscolhido);


        //filtrar os filmes
        //if (generoEscolhido === 'todos') {
        //    //mostrar todos os filmes
        //    $('.col').show();
        //} else {
        //    //esconder todos os filmes
        //    $('.col').hide();
        //    //mostrar apenas os filmes do genero clicado
        //    $(`.col:has(.card-body:contains('${generoEscolhido}'))`).show();
        //}
    });


   
    BuscarFilmes("todos");
    function BuscarFilmes(generoEscolhido) {

        url = "/filme";
        if (generoEscolhido != 'todos') {
            url = url + "/" + generoEscolhido;
        }

        $.ajax({
            url: urlBase + url,
            type: "GET",
            contentType: "application/json",

            success: function(dados) {

                //selecione a div
                const divFilmes = $('#filmes-container');
                divFilmes.html("");

                dados.forEach(filme => {

                    corBagde = "success";
                    if (filme.classificacao == "+18") {
                        corBagde = "danger";
                    } else if (filme.classificacao == "Kids") {
                        corBagde = "primary";
                    }

                    const card = `<div class="col">
                <div class="card bg-dark border-secondary h-100">
                    <img src="${filme.urlImagem}" class="card-img-top" alt="${filme.titulo}" style="height: 250px; object-fit: cover;">
                    <div class="card-body">
                        <h5 class="card-title">${filme.titulo}</h5>
                        <p class="card-text text-muted">${filme.genero}</p>
                        <p class="card-text small text-muted">${filme.produtora}</p>
                        <span class="badge bg-${corBagde}">${filme.classificacao}</span>
                    </div>
                </div>
            </div>`;

                    divFilmes.append(card);
                });
            },
            error: function(erro) {
                console.log('Erro ao carregar filmes!');
            }
        }); 

    }

    //Busca por título. Quando teclar algo na caixa de busca
    $('#busca-filme').on('keyup', function () {

            // Pega o valor do campo de busca e converte para minúsculas
            const termoBusca = $(this).val().toLowerCase();

            // Percorrer cada div de filme na div 'filmes-container'
            $('#filmes-container div').each(function () {
                // Pega o texto do título do filme
                const tituloFilme = $(this).find('.card-title').text().toLowerCase();

                // Verifica se o título do filme inclui o termo de busca
                if (tituloFilme.includes(termoBusca)) {
                    // Se sim, mostra o card
                    $(this).show();
                } else {
                    // Se não, oculta o card
                    $(this).hide();
                }
            });
    });

    //Envio do Formulário
    $('#form-contact').submit(function (event) {

        // Evita que o formulário recarregue a página
        event.preventDefault();

        //monta o json de envio
        let dadosEmail = {
            nomeRemetente: $('#nome').val(),
            emailRemetente: $('#emailRemetente').val(),
            telefone: $('#telefone').val(),
            assunto: $('#assunto').val(),
            mensagem: $('#mensagem').val(),
        }

        console.log(dadosEmail);

        //envia os dados para api de email
        $.ajax({
            url: urlBase + "/Email",
            type: "POST",
            contentType: 'application/json',
            data: JSON.stringify(dadosEmail), //Dados (json)
            success: function (dados) {
                alert('E-mail enviado com sucesso!');
                //limpa os campos
                $('#nome').val(''),
                    $('#emailRemetente').val(''),
                    $('#telefone').val(''),
                    $('#assunto').val(''),
                    $('#mensagem').val('')
            },
            error: function (erro) {
                console.log("Ocorreu um erro ao enviar o e-mail.")
            }
        });
    });

});
