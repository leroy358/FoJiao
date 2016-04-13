window.addEventListener('load', uploader);
window.addEventListener('load', delete_);
var num;

function delete_() {
    var articlePic = document.getElementById('headlinePic');
    var deleteImg = document.querySelectorAll('.deleteImg');
    var image = document.querySelectorAll('.image');
    var imgContainer = document.querySelectorAll('.imgContainer');
    num = document.querySelectorAll('.imgContainer').length;
    for (var i = 0; i < deleteImg.length; i++) {
        (function(i) {
            deleteImg[i].onclick = function() {
                var src_ = image[i].getAttribute('src')+' ';
                imgUnit.removeChild(imgContainer[i]);
                articlePic.value = articlePic.value.replace(src_, '');
                num--;
                if (num == 0) {
                    imgUnit.style.height = 0 + 'px';
                }
            }
        })(i)
    }
}

function uploader() {
    var image = document.querySelectorAll('.image');
    var articlePic = document.getElementById('headlinePic');
    var imgUnit = document.getElementById('imgUnit');
    if(image.length>0){
        imgUnit.style.height='auto';
    }
    for (var i = 0; i < image.length; i++) {
        articlePic.value = articlePic.value + image[i].getAttribute('src')+' ';
    }
    article = simple.uploader.create({
        appendTo: document.getElementById("upload_headlinePic"),
        url: "/Upload/Images", //上传地址
        valueBindTo: document.getElementById("articlePic"),
        multiple: true,
        limit: {
            fileFormat: "jpg|jpeg|png" //文件格式
        },
        uiText: {
            selectFile: "上传图片",
            limit: "没选对文件"
        },
        onEvent: {
            onSelectFile: function(files, limitValidateResult) {
                a_onSelectFile_a = files;
                a_onSelectFile_b = limitValidateResult;
            },
            onProgress: function(e) {
                a_onProgress_a = e.loaded;
            },
            onError: function(e) {
                a_onError_a = e;
            },
            onCancelWhenUpload: function() {
                a_onCancelWhenUpload_a = 111;
            },
            onCancelWhenComplete: function() {
                a_onCancelWhenComplete_a = 111;
            },
            onComplete: function(files, returnResult) {
                a_onComplete_a = files;
                a_onComplete_b = returnResult;
                var imgSrc = returnResult.split(" ");

                num += imgSrc.length;
                var articlePic = document.getElementById('headlinePic');
                
                
                for (var i = 0; i < imgSrc.length; i++) {
                    var imgUnit = document.getElementById('imgUnit');
                    var imgContainer = document.createElement('div');
                    imgContainer.setAttribute('class', 'imgContainer');
                    var image = document.createElement('img');
                    image.setAttribute('class', 'image');
                    imgContainer.appendChild(image);
                    var deleteImg = document.createElement('div');
                    deleteImg.setAttribute('class', 'deleteImg');
                    imgContainer.appendChild(deleteImg);
                    imgUnit.appendChild(imgContainer);
                    imgUnit.style.height = 'auto';
                    image.src = imgSrc[i];
                }
                articlePic.value = articlePic.value + returnResult + ' ';
                                        
                    image = document.querySelectorAll('.image');
                    deleteImg = document.querySelectorAll('.deleteImg');
                    imgContainer= document.querySelectorAll('.imgContainer');
                    for (var i = 0; i < deleteImg.length; i++) {
                        (function(i) {
                            deleteImg[i].onclick = function() {
                                var src_ = image[i].getAttribute('src')+' ';
                                imgUnit.removeChild(imgContainer[i]);
                                articlePic.value = articlePic.value.replace(src_, '');
                                num--;
                                if (num == 0) {
                                    imgUnit.style.height = 0 + 'px';
                                }
                            }
                        })(i)
                    
                }
                return false;
            }
        }
    });

    article.setData({
        multiple: true,
        limit: {
            fileSizeMin: 1,
            fileSizeMax: 2048
        }
    });

    ab = article.getData();

}