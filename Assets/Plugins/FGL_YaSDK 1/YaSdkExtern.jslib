mergeInto(LibraryManager.library, {
 ShowFullscreenAds: function () {
    ShowFullscreenAds();
  },

  ShowRewardedAds: function(placement) {
    ShowRewardAds(placement);
  },
  
  GetString: function (key){
  return GetData(key);
  },
  SetString: function(key, value){
  SetString(key, value);
  },
  GetLang: function(){
  var urlParams = window.location.search.replace('?', ''); 
  var returnStr = new URLSearchParams(urlParams).get("lang");
  if(!returnStr)returnStr = "ru";
  var bufferSize = lengthBytesUTF8(returnStr) + 1;
  var buffer = _malloc(bufferSize);
  stringToUTF8(returnStr, buffer, bufferSize);
  return buffer;
  }
});