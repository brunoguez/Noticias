let dxComp = {}, dataSource = {};

$(async () => {
    await carregarDataSourceNoticias();
    carregaDxComp();
});

function carregarDataSourceNoticias() {
    return $.get("Noticias/api/GetNoticias")
        .done(a => {
            console.log(a);
            dataSource.noticias = a.noticias;
            dataSource.categoriaList = a.categoriaList;
        })
        .fail(() => {
            //TODO: desenvolver erro
        });
}

function carregaDxComp() {
    dxComp.categorias = new DevExpress.ui.dxTagBox("#categorias", {
        dataSource: dataSource.categoriaList,
    });
    dxComp.categorias.option('value', dataSource.categoriaList);
}