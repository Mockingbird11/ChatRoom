function AjaxPost(url, postData, callBack) {
    try {
        $.ajax({
            type: 'post',
            dataType: 'json',
            url: RootPath() + url,
            data: postData,
            cache: false,
            async: false,
            success: function (data) {
                callBack(data);
            },
            error: function (data) {
                callBack(data);
            }
        });
    } catch (exception) {
        return false;
    }

}

function RootPath() {
    var strFullPath = window.document.location.href;
    var strPath = window.document.location.pathname;
    var pos = strFullPath.indexOf(strPath);
    var prePath = strFullPath.substring(0, pos);
    var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
    //return (prePath + postPath);如果发布IIS，有虚假目录用用这句
    return (prePath);
}