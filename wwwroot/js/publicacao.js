let dxComp = {}, dataSource = {};


$(async () => {

    DevExpress.setTemplateEngine({
        compile: (element) => $(element).html(),
        render: (template, data) => mustache.render(template, data),
    });

    await carregarDataSourcePublicacao();
    carregaDxComp();
});

function carregarDataSourcePublicacao() {
    return $.get("api/GetPublicacao")
        .done(a => {
            console.log(a);
            dataSource.categorias = a.categorias;
            dataSource.noticias = a.noticias;
        })
        .fail(a => {
            console.error(a);
        });
}

function carregaDxComp() {

    dxComp.titulo = new DevExpress.ui.dxTextBox("#titulo", {
        label: "Título",
        width: 400,
        labelMode: "floating",
    });
    new DevExpress.ui.dxValidator("#titulo", {
        validationGroup: "validator",
        validationRules: [{
            type: 'required',
            message: 'O título é necessário para a publicação',
        }],
    });

    dxComp.imagem = new DevExpress.ui.dxFileUploader("#imagem", {
        selectButtonText: 'Selecione sua imagem',
        labelText: '',
        accept: 'image/*',
        uploadUrl: "api/noticia/imagem",
        uploadMode: 'useForm',
    })

    dxComp.noticia = new DevExpress.ui.dxTextArea("#noticia", {
        label: "Notícia",
        width: 700,
        height: 110,
        labelMode: "floating",
    });
    new DevExpress.ui.dxValidator("#noticia", {
        validationGroup: "validator",
        validationRules: [{
            type: 'required',
            message: 'A notícia é necessária para a publicação',
        }],
    });

    dxComp.categoria = new DevExpress.ui.dxLookup("#categoria", {
        width: 400,
        dataSource: dataSource.categorias,
        valueExpr: 'id',
        displayExpr: 'descricao',
        labelMode: "floating",
        label: "Categoria",
    });
    new DevExpress.ui.dxValidator("#categoria", {
        validationGroup: "validator",
        validationRules: [{
            type: 'required',
            message: 'A categoria é necessária para a publicação',
        }],
    });

    dxComp.publicada = new DevExpress.ui.dxSwitch("#publicada", {
        width: 100,
        switchedOnText: "PUBLICADA",
        switchedOffText: "DESATIVADA",
    });

    let limpar = () => {
        dxComp.titulo.option('value', null);
        dxComp.noticia.option('value', null);
        dxComp.categoria.option('value', null);
        dxComp.publicada.option('value', true);

        dxComp.titulo.option('validationStatus', 'valid');
        dxComp.noticia.option('validationStatus', 'valid');
        dxComp.categoria.option('validationStatus', 'valid');
        dxComp.imagem.reset();
    };

    dxComp.limpar = new DevExpress.ui.dxButton("#limpar", {
        text: "Limpar",
        width: 100,
        onClick: limpar,
    });

    dxComp.publicar = new DevExpress.ui.dxButton("#publicar", {
        text: "Publicar",
        width: 100,
        onClick: e => {
            console.log(e, DevExpress.validationEngine.validateGroup("validator"));
            if (!DevExpress.validationEngine.validateGroup("validator").isValid) return;

            ////TODO: enviar create usuário

            //$.post('dsadsda', { nome: dxComp.nome.option('value'), email: dxComp.email.option('value'), senha: dxComp.senha.option('value') })
            //    .done(() => {
            //        //TODO: deu certo ir para próxima página
            //    })
            //    .fail(() => {
            //        //TODO: Erro
            //    })
        },
    });


}

//<div id="lookUpNoticias"></div>
//    <div id="titulo"></div>
//    <div id="imagem"></div>
//    <div id="noticia"></div>
//    <div id="categoria"></div>
//    <div id="publicada"></div>
//    <div class="mt-4">
//        <div id="limpar"></div>
//        <div id="publicar"></div>
//    </div>