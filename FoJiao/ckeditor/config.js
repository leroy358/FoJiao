/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
    // config.uiColor = '#AADC6E';

    config.height = "500";//文本域高度

    //config.skins = "bootstrapck";

    config.filebrowserImageBrowseUrl = '/ckfinder/ckfinder.html?Type=Images';
    config.filebrowserFlashBrowseUrl = '/ckfinder/ckfinder.html?Type=Flash';
    config.filebrowserVideoBrowseUrl = '/ckfinder/ckfinder.html?Type=Video';
    config.filebrowserUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
    config.filebrowserFlashUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';
    config.filebrowserVideoUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Video';
    config.filebrowserWindowWidth = '800';  //“浏览服务器”弹出框的size设置
    config.filebrowserWindowHeight = '500';
    config.toolbar = "Full";
    config.toolbar_Full = [
      ['Save','NewPage','Preview'],
      ['Undo','Redo','-','Find','Replace','-','SelectAll','RemoveFormat'],   
      ['Image'],
      ['Bold', 'Italic', 'Underline', 'Strike', 'FontSize'],
      ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
      ['Link','Unlink','Anchor']    
      // ['Save', 'NewPage', 'Preview'],
      //['Cut', 'Copy', 'Paste', 'PasteText'],
      //['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
      //['Checkbox', 'Radio', 'TextField', 'Textarea', 'Select', 'Button'],
      //['Image', 'Flash', 'Video', 'Table', 'HorizontalRule', 'SpecialChar'],
      // '/',
      //['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
      //['Format', 'Font', 'FontSize'],
      //['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent'],
      //['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
      //['TextColor', 'BGColor'],
      //['Link', 'Unlink', 'Anchor']
    ];
    //config.extraPlugins = 'video';
};
