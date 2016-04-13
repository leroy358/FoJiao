CKEDITOR.dialog.add('flvPlayerDialog', function (editor) {
    var lang = editor.lang.video;
    return {
        title: '视频属性',
        minWidth: 400,
        minHeight: 200,

        contents :
		[
			{
			    id : 'info',
			    elements :
				[
					{
					    type : 'hbox',
					    widths: [ '', '100px'],
					    children : [
							{
							    type : 'text',
							    id : 'poster',
							    label : lang.poster,
							    commit : commitValue,
							    setup : loadValue,
							    onChange : function()
							    {
							        var dialog = this.getDialog(),
										newUrl = this.getValue();

							        //Update preview image
							        if ( newUrl.length > 0 )	//Prevent from load before onShow
							        {
							            dialog = this.getDialog();
							            var preview = dialog.previewImage;

							            preview.on( 'load', onImgLoadEvent, dialog );
							            preview.on( 'error', onImgLoadErrorEvent, dialog );
							            preview.on( 'abort', onImgLoadErrorEvent, dialog );
							            preview.setAttribute( 'src', newUrl );
							        }
							    }
							},
							{
							    type : 'button',
							    id : 'browse',
							    hidden : 'true',
							    style : 'display:inline-block;margin-top:10px;',
							    filebrowser :
								{
								    action : 'Browse',
								    target: 'info:poster',
								    url: editor.config.filebrowserImageBrowseUrl || editor.config.filebrowserBrowseUrl
								},
							    label : editor.lang.common.browseServer
							}]
					},
					{
					    type : 'hbox',
					    widths: [ '33%', '33%', '33%'],
					    children : [
							{
							    type : 'text',
							    id : 'width',
							    label : editor.lang.common.width,
							    'default' : 400,
							    validate : CKEDITOR.dialog.validate.notEmpty( lang.widthRequired ),
							    commit : commitValue,
							    setup : loadValue
							},
							{
							    type : 'text',
							    id : 'height',
							    label : editor.lang.common.height,
							    'default' : 300,
							    validate : CKEDITOR.dialog.validate.notEmpty(lang.heightRequired ),
							    commit : commitValue,
							    setup : loadValue
							},
							{
							    type : 'text',
							    id : 'id',
							    label : 'Id',
							    commit : commitValue,
							    setup : loadValue
							}
					    ]
					},
					{
					    type : 'hbox',
					    widths: [ '', '100px', '75px'],
					    children : [
							{
							    type : 'text',
							    id : 'src0',
							    label : lang.sourceVideo,
							    commit : commitSrc,
							    setup : loadSrc
							},
							{
							    type : 'button',
							    id : 'browse',
							    hidden : 'true',
							    style : 'display:inline-block;margin-top:10px;',
							    filebrowser :
								{
								    action : 'Browse',
								    target: 'info:src0',
								    url: editor.config.filebrowserVideoBrowseUrl || editor.config.filebrowserBrowseUrl
								},
							    label : editor.lang.common.browseServer
							},
							{
							    id : 'type0',
							    label : lang.sourceType,
							    type : 'select',
							    'default' : 'video/mp4',
							    items :
								[
									[ 'MP4', 'video/mp4' ],
									[ 'WebM', 'video/webm' ]
								],
							    commit : commitSrc,
							    setup : loadSrc
							}]
					},

					{
					    type : 'hbox',
					    widths: [ '', '100px', '75px'],
					    children : [
							{
							    type : 'text',
							    id : 'src1',
							    label : lang.sourceVideo,
							    commit : commitSrc,
							    setup : loadSrc
							},
							{
							    type : 'button',
							    id : 'browse',
							    hidden : 'true',
							    style : 'display:inline-block;margin-top:10px;',
							    filebrowser :
								{
								    action : 'Browse',
								    target: 'info:src1',
								    url: editor.config.filebrowserVideoBrowseUrl || editor.config.filebrowserBrowseUrl
								},
							    label : editor.lang.common.browseServer
							},
							{
							    id : 'type1',
							    label : lang.sourceType,
							    type : 'select',
							    'default':'video/webm',
							    items :
								[
									[ 'MP4', 'video/mp4' ],
									[ 'WebM', 'video/webm' ]
								],
							    commit : commitSrc,
							    setup : loadSrc
							}]
					}
				]
			}

		],

        onOk: function () {
            var dialog = this;
            var url = dialog.getValueOf('flv', 'flvLink');

            var video = editor.document.createElement('video');
            video.setAttribute('src', url);
            editor.insertElement(video);
        }
    };
});