

let dxComp = {};
let limpar = e => {
    dxComp.nome.option('value', null);
    dxComp.email.option('value', null);
    dxComp.senha.option('value', null);

    dxComp.nome.option('validationStatus', 'valid');
    dxComp.email.option('validationStatus', 'valid');
    dxComp.senha.option('validationStatus', 'valid');
    dxComp.foto.reset();
};

function clickSlideBar() {
    $(".selecao").on("click", e => {
        $(".selecao").removeClass("active");
        $(e.target).addClass("active");
    })
}

function init() {
    dxComp.nome = new DevExpress.ui.dxTextBox("#nome", {
        label: "Nome",
        width: 400,
        labelMode: "floating",
    });

    new DevExpress.ui.dxValidator("#nome", {
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
        validationRules: [{
            type: 'required',
            message: 'E-mail é necessário para o cadastro',
        }],
    });

    dxComp.senha = new DevExpress.ui.dxTextBox("#senha", {
        label: "Senha",
        width: 200,
        labelMode: "floating",
    });

    new DevExpress.ui.dxValidator("#senha", {
        validationRules: [{
            type: 'required',
            message: 'Sanha é necessária para o cadastro',
        }],
    });

    dxComp.foto = new DevExpress.ui.dxFileUploader("#foto", {
        selectButtonText: 'Selecione sua foto',
        labelText: '',
        accept: 'image/*',
        uploadMode: 'useForm',
    })

    dxComp.limpar = new DevExpress.ui.dxButton("#limpar", {
        text: "Limpar",
        width: 100,
        onClick: limpar,
    });

    dxComp.cadastrar = new DevExpress.ui.dxButton("#cadastrar", {
        text: "Cadastrar",
        width: 100,
        onClick: e => {
            console.log(e);
            dxComp.nome.validationRequest.fire();
            dxComp.email.validationRequest.fire();
            dxComp.senha.validationRequest.fire();
            if (dxComp.nome.option('validationStatus') !== 'valid' &&
                dxComp.email.option('validationStatus') !== 'valid' &&
                dxComp.senha.option('validationStatus') !== 'valid') return;

            //TODO: enviar create usuário

            $.post('dsadsda', { nome: dxComp.nome.option('value'), email: dxComp.email.option('value'), senha: dxComp.senha.option('value') })
                .done(() => {
                    //TODO: deu certo ir para próxima página
                })
                .fail(() => {
                    //TODO: Erro
                })
        },
    });
}


$(() => {
    init();
    clickSlideBar();
});