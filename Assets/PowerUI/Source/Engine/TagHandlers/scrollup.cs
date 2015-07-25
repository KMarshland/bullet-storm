//--------------------------------------
//               PowerUI
//
//        For documentation or 
//    if you have any issues, visit
//        powerUI.kulestar.com
//
//    Copyright � 2013 Kulestar Ltd
//          www.kulestar.com
//--------------------------------------

namespace PowerUI{
	
	/// <summary>
	/// Handles the scroll up button on scrollbars.
	/// </summary>
	
	public class ScrollUpTag:HtmlTagHandler{
		
		public override string[] GetTags(){
			return new string[]{"scrollup"};
		}
		
		public override bool SelfClosing(){
			return true;
		}
		
		public override Wrench.TagHandler GetInstance(){
			return new ScrollUpTag();
		}
		
		public override bool OnClick(UIEvent clickEvent){
			// Get the scroll bar:
			InputTag scroll=((InputTag)(Element.parentNode.Handler));
			// And scroll it:
			scroll.ScrollBy(-1);
			// Prevent bubbling:
			clickEvent.stopPropagation();
			
			return true;
		}
		
	}
	
}