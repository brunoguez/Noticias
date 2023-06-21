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
            dataSource.user = a.user;
        })
        .fail(a => {
            console.error(a);
        });
}

function carregaDxComp() {

    dxComp.lookUpNoticias = new DevExpress.ui.dxLookup("#lookUpNoticias", {
        width: 700,
        wrapItemText: true,
        dataSource: dataSource.noticias,
        valueExpr: 'id',
        displayExpr: 'desc',
        labelMode: "floating",
        label: "Publicações anteriores",
        showClearButton: true,
    })

    dxComp.titulo = new DevExpress.ui.dxTextBox("#titulo", {
        label: "Título",
        width: 800,
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
        uploadUrl: "/noticias/api/CreatePublicacao",
        uploadMode: 'useForm',
    })

    dxComp.noticia = new DevExpress.ui.dxTextArea("#noticia", {
        label: "Notícia",
        width: 800,
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
        wrapItemText: true,
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
        value: true,
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
        onClick: async e => {
            console.log(e, DevExpress.validationEngine.validateGroup("validator"));
            if (!DevExpress.validationEngine.validateGroup("validator").isValid) return;

            let reqUrl = '/noticias/api/CreatePublicacao';
            if (dxComp.lookUpNoticias.option('value') !== null) reqUrl = '/noticias/api/UpdatePublicacao/' + dxComp.lookUpNoticias.option('value');

            let data = {
                AutorId: dataSource.user.id,
                Titulo: dxComp.titulo.option('value'),
                URL_imagem: dxComp.imagem.option('value').length == 0 ? "" : dxComp.imagem.option('value')[0].name,
                Texto: dxComp.noticia.option('value'),
                Publicada: dxComp.publicada.option('value'),
                CategoriaId: dxComp.categoria.option('value'),
            }

            let newNoticia, idForm;

            await $.post(reqUrl, data)
                .done(a => {
                    console.log(a);
                    newNoticia = a;
                    idForm = String(a.id);
                })
                .fail(a => {
                    console.error(a);
                })

            if (dxComp.imagem.option('value').length > 0) {
                let formData = new FormData();
                formData.append('image', dxComp.imagem.option('value')[0]);
                formData.append('id', idForm);

                $.ajax({
                    url: '/noticias/api/SaveImagemPublicacao',
                    type: 'POST',
                    data: formData,
                    processData: false,
                    contentType: false,
                })
                    .done(a => {
                        console.log(a);
                    })
                    .fail(a => {
                        console.error(a);
                    })
            }

        },
    });


}

