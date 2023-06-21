

let dxComp = {}, dataSources = {
    categorias: {}
};
let limpar = e => {
    dxComp.nome.option('value', null);
    dxComp.email.option('value', null);
    dxComp.senha.option('value', null);

    dxComp.nome.option('validationStatus', 'valid');
    dxComp.email.option('validationStatus', 'valid');
    dxComp.senha.option('validationStatus', 'valid');
    dxComp.foto.reset();
};

async function carregaDataSources() {
    return $.get("noticias/api/GetCategorias")
        .done(a => {
            console.log(a);
            dataSources.categorias = a;
        })
        .fail(a => {
            console.log(a);
        });
}

function clickSlideBar() {
    $(".selecao").on("click", e => {
        $(".selecao").removeClass("active");
        $(e.target).addClass("active");
    })
}

async function init() {
    dxComp.nome = new DevExpress.ui.dxTextBox("#nome", {
        label: "Nome",
        width: 400,
        labelMode: "floating",
    });

    new DevExpress.ui.dxValidator("#nome", {
        validationGroup: "validator",
        validationRules: [{
            type: 'required',
            message: 'Nome é necessário para o cadastro',
        }],
    });

    dxComp.email = new DevExpress.ui.dxTextBox("#email", {
        label: "E-mail",
        width: 450,
        labelMode: "floating",
    });

    new DevExpress.ui.dxValidator("#email", {
        validationGroup: "validator",
        validationRules: [{
            type: 'required',
            message: 'E-mail é necessário para o cadastro',
        }, {
            type: 'email',
            message: 'E-mail inválido',
        },],
    });

    dxComp.senha = new DevExpress.ui.dxTextBox("#senha", {
        label: "Senha",
        width: 200,
        labelMode: "floating",
    });

    new DevExpress.ui.dxValidator("#senha", {
        validationGroup: "validator",
        validationRules: [{
            type: 'required',
            message: 'Sanha é necessária para o cadastro',
        }],
    });

    dxComp.foto = new DevExpress.ui.dxFileUploader("#foto", {
        selectButtonText: 'Selecione sua foto',
        labelText: 'Imagem de perfil',
        accept: 'image/*',
        uploadUrl: "api/user/foto",
        uploadMode: 'useForm',
        onValueChanged: e => {
            if (e.value.length < 0) return;
            const leitor = new FileReader();
            leitor.addEventListener('load', function () {
                $("#imagemEscolhida").attr('src', leitor.result);
            });
            leitor.readAsDataURL(e.value[0]);
        },
    })

    await dataSources.categorias._get;

    dxComp.tagCategorias = new DevExpress.ui.dxTagBox("#tagCategorias", {
        dataSource: dataSources.categorias,
        label: "Categorias favoritas",
        width: 600,
        labelMode: "floating",
        displayExpr: "descricao",
        valueExpr: "id",
    });

    dxComp.limpar = new DevExpress.ui.dxButton("#limpar", {
        text: "Limpar",
        width: 100,
        onClick: limpar,
    });

    dxComp.cadastrar = new DevExpress.ui.dxButton("#cadastrar", {
        text: "Cadastrar",
        width: 100,
        onClick: async (e) => {
            console.log(e);
            if (!DevExpress.validationEngine.validateGroup("validator").isValid) return;

            let data = {
                user: {
                    nome: dxComp.nome.option('value'),
                    email: dxComp.email.option('value'),
                    password: dxComp.senha.option('value'),
                    foto: dxComp.foto.option('value')[0].name
                }
            }

            if (dxComp.tagCategorias.option('selectedItems').length > 0) {
                data.categorias = [];
                $.each(dxComp.tagCategorias.option('selectedItems'), (a, b) => {
                    data.categorias.push(b.id);
                })
            }
            console.log(data);

            let idForm;

            await $.post('api/user/create', data)
                .done((e) => {
                    console.log(e);
                    idForm = String(e.user.id);
                    //TODO: deu certo ir para próxima página
                })
                .fail((e) => {
                    console.log(e);
                    //TODO: Erro
                })

            if (dxComp.foto.option('value').length > 0) {
                let formData = new FormData();
                formData.append('image', dxComp.foto.option('value')[0]);
                formData.append('id', idForm);

                $.ajax({
                    url: 'api/user/foto',
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


$(() => {
    dataSources.categorias._get = carregaDataSources();
    init();
    clickSlideBar();
});