$(() => {
    init();
})

function init() {

    $("#entrar").on('click', (b, c) => {
        console.log(b, c)
        let user = $("#inputEmail").val();
        let pw = $("#inputPassword").val();

        if (user === "" || pw === "") {
            new PNotify({
                type: "error",
                delay: 3000,
                text: "Digite as credenciais antes de continuar",
            })
            return;
        }

        $.post(`/api/home/auth?Email=${user}&Password=${pw}`)
            .done(a => {
                console.log(a)
                //window.location.href = document.location.origin;
            })
            .fail(a => {
                new PNotify({
                    type: "error",
                    delay: 3000,
                    text: "E-mail ou senha incorretos",
                    title: "Erro no login"
                })
            });
    })

    $("#inscrever").on('click', (b, c) => {
        console.log(b, c);
        window.location.href = document.location.origin + "\\Cadastro";
    })

    let dxGoNoticias = new DevExpress.ui.dxButton("#goNoticias", {
        text: "Ir para as Noticias",
        onClick: () => window.location.href = document.location.origin + "\\Noticias",
    });

}