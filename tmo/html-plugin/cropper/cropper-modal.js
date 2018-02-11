/***
     ͼ����жԻ���
     @elModal  :  �Ի���Ԫ��
     @option:����[{img:չʾimgԪ��,fld:���inputԪ��,x:���п��,y:���и߶�},{...}];



     ʾ����

     //Html չʾ�㣬 ��imgԪ�ر����ʱͼ����жԻ��򵯳�
     <div>
        <img src='....' id='img1'/>
        <input id='inp1'>

        <img src='...' id='img2'/>
        <input id='inp2'>
     <div>


     //���жԻ���
     <div class="modal" id="modalCropper" aria-labelledby="modalLabel" role="dialog" tabindex="-1">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="modalLabel">����ͼ��</h4>
                </div>
                <div class="modal-body">
                    <div style="background-color: azure; max-height: 350px; overflow: hidden">
                        <img id="croImage" src="" alt="Picture" style="left: 0px; top: 0px; width: 100%" />
                        <input type="file" id="cropFile" style="display: none" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" id="btnOk">ȷ��</button>
                    <button type="button" class="btn btn-default" id="btnCancel">ȡ��</button>
                </div>
            </div>
        </div>
    </div>


    //�����ű�
    <script>
       new CropperDialog(modalCropper, [{ img: img1, fld: inp1, x: 400, y: 400 }, {
                    img: img2, fld: inp2, x: 1000, y: 333}] ;
   </script>

***/

function CropperDialog(elModal, options) {

    var curOpt;
    var isUploading = false;
    var $CropFile = $(elModal).find("#cropFile");
    var $CroImage = $(elModal).find("#croImage");

    for (var i = 0; i < options.length; i++) {
        var opt = options[i];
        opt.img.opt = opt;
        $(opt.img).click(insertImage);
    }

    $(elModal).find("#btnOk").click(function () {
        cropperOk();
    });

    $(elModal).find("#btnCancel").click(function () {
        closeModal();
    })

    $CropFile.change(function () {
        var cropFile = this;
        if (cropFile.files.length == 0)
            return;
        $(elModal).modal("show");
        URL = (window.webkitURL || window.URL);
        url = URL.createObjectURL(cropFile.files[0]);
        // croImage.style.width = '100%';
        if ($CroImage.data('cropper')) {
            $CroImage.cropper("reset").cropper('replace', url).cropper("setAspectRatio", curOpt.x / curOpt.y);
        }
        else {
            $CroImage.attr("src", url);
            cropper(curOpt.x, curOpt.y);
        }

    });


    function cropperOk() {
        isUploading = true;
        // alert("upload");
        var ca = $CroImage.cropper('getCroppedCanvas', { width: curOpt.x });

        var data = ca.toDataURL("image/jpeg");//"image/png"
        //glbInput.value = data;

        var formData = new FormData();
        formData.append('upfile', data);
        $.postFormData(formData, "/ueditor/base64.ashx?action=dataurl", function (json) {
            $(curOpt.fld).val(json.url);
            $(curOpt.img).attr("src", json.url);
            $(elModal).trigger("ok", [curOpt.img,curOpt.fld]);
            curOpt = null;
            isUploading = false;
        });
        $(elModal).modal('hide');
        $CropFile.val("");

    }

    function cropper(x, y) {
        $CroImage.cropper({
            aspectRatio: x / y,//16 / 9, //4/3,
            viewMode: 1,
            dragMode: 'move',
            autoCropArea: 1,
            restore: false,
            modal: true,
            guides: true,
            highlight: false,
            cropBoxMovable: true,
            cropBoxResizable: false,
            center: false,
            autoCrop: true
        });
        $CroImage.cropper('moveTo', 0, 0);
    }

    function closeModal() {
        $(elModal).modal('hide');
        $CropFile.val("");
    }

    function insertImage() {
        if (isUploading) {
            alert("��ȴ��ϴ�ͼƬ");
            return;
        }
        curOpt = this.opt;
        $CropFile.click();
    }
}
