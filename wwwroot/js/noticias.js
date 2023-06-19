let dxComp = {}, dataSource = { categoriaListObject: [] };


$(async () => {

    DevExpress.setTemplateEngine({
        compile: (element) => $(element).html(),
        render: (template, data) => mustache.render(template, data),
    });

    await carregarDataSourceNoticias();
    carregaDxComp();
});

function carregarDataSourceNoticias() {
    return $.get("Noticias/api/GetNoticias")
        .done(a => {
            console.log(a);
            dataSource.noticias = a.noticias;

            dataSource.noticias.forEach(a => {
                a.noticiasList.forEach(b => {
                    b.dataString = new Date(b.dataPublicacao).toLocaleString();
                })
            })
            dataSource.categoriaList = a.categoriaList;
        })
        .fail(() => {
            //TODO: desenvolver erro
        });
}

function carregaDxComp() {

    dataSource.categoriaList.forEach(i => dataSource.categoriaListObject.push({ Categoria: i }));

    dxComp.categorias = new DevExpress.ui.dxDataGrid("#categorias", {
        dataSource: dataSource.noticias,
        width: 250,
        rowDragging: {
            allowReordering: true,
            onReorder: e => {
                const visibleRows = e.component.getVisibleRows();
                const toIndex = dataSource.noticias.findIndex((item) => item.categoria === visibleRows[e.toIndex].data.categoria);
                const fromIndex = dataSource.noticias.findIndex((item) => item.categoria === e.itemData.categoria);

                dataSource.noticias.splice(fromIndex, 1);
                dataSource.noticias.splice(toIndex, 0, e.itemData);

                e.component.refresh();

                if (dxComp.noticiasGrid !== null) {
                    dxComp.noticiasGrid.getDataSource().reload();
                }
            }
        },
        columns: [{
            dataField: 'categoria', caption: "Categorias"
        }]
    });

    dxComp.noticiasGrid = new DevExpress.ui.dxAccordion("#gridNoticias", {
        dataSource: dataSource.noticias,
        itemTitleTemplate: $("#title"),
        itemTemplate: $("#content"),
        collapsible: true,
        multiple: true,
    })
}
