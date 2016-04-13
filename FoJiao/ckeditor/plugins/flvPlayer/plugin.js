( function() {
CKEDITOR.plugins.add('flvPlayer', {
    lang: ['en'],
    icons: 'flvPlayer',
    init: function (editor) {
        editor.addCommand('flvPlayer', new CKEDITOR.dialogCommand('flvPlayerDialog'));

        editor.ui.addButton('flvPlayer', {
            label: '视频',
            command: 'flvPlayer',
            toolbar: '插入flv视频'
        });

        CKEDITOR.dialog.add('flvPlayerDialog', this.path + 'dialogs/flvPlayer.js');
    }
});
var en = {
    toolbar: '视频',
    dialogTitle: '视频属性',
    fakeObject: '视频',
    properties: '编辑视频',
    widthRequired: '视频宽度不能为空',
    heightRequired: '视频高度不能为空',
    poster: '缩略图',
    sourceVideo: 'URL',
    sourceType: '视频类别',
    linkTemplate: '<a href="%src%">%type%</a> ',
    fallbackTemplate: 'Your browser doesn\'t support video.<br>Please download the file: %links%'
};
if (CKEDITOR.skins)
{
    en = { video : en} ;
}

// Translations
CKEDITOR.plugins.setLang( 'video', 'en', en );
})();