mergeInto(LibraryManager.library, {

  VibrateEx: function () 
  {
    if (window.navigator && window.navigator.vibrate) 
	{
		navigator.vibrate(1000);
	} 
  },
});