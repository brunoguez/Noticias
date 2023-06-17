$(() => {
    init();
})

function init() {
    
    $(document).on('submit', (b,c) => {
        console.log(b,c)
        let user = $("#inputEmail").val();
        let pw = $("#inputPassword").val();
        $.post(`/api/home/auth?Email=${user}&Password=${pw}`, a => {
            console.log(a)
            window.location.href = document.location.origin;
        })
    })

}