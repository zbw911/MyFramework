//密码强度;
function PasswordStrength(showed){	
	this.showed = (typeof(showed) == "boolean")?showed:true;
	this.styles = new Array();	
	this.styles[0] = {backgroundColor:"#EBEBEB",borderLeft:"solid 1px #FFFFFF",borderRight:"solid 1px #BEBEBE",borderBottom:"solid 1px #BEBEBE"};	
	this.styles[1] = {backgroundColor:"#FF4545",borderLeft:"solid 1px #FFFFFF",borderRight:"solid 1px #BB2B2B",borderBottom:"solid 1px #BB2B2B"};
	this.styles[2] = {backgroundColor:"#FFD35E",borderLeft:"solid 1px #FFFFFF",borderRight:"solid 1px #E9AE10",borderBottom:"solid 1px #E9AE10"};
	this.styles[3] = {backgroundColor:"#95EB81",borderLeft:"solid 1px #FFFFFF",borderRight:"solid 1px #3BBC1B",borderBottom:"solid 1px #3BBC1B"};
	
	this.labels= ["弱","中","强"];

	this.divName = "pwd_div_"+Math.ceil(Math.random()*100000);
	this.minLen = 5;
	
	this.width = "150px";
	this.height = "16px";
	
	this.content = "";
	
	this.selectedIndex = 0;
	
	this.init();	
}
PasswordStrength.prototype.init = function(){
	var s = '<table cellpadding="0" id="'+this.divName+'_table" cellspacing="0" style="width:'+this.width+';height:'+this.height+';">';
	s += '<tr>';
	for(var i=0;i<3;i++){
		s += '<td id="'+this.divName+'_td_'+i+'" width="33%" align="center"><span style="font-size:1px">&nbsp;</span><span id="'+this.divName+'_label_'+i+'" style="display:none;font-family: Courier New, Courier, mono;font-size: 12px;color: #000000;">'+this.labels[i]+'</span></td>';
	}	
	s += '</tr>';
	s += '</table>';
	this.content = s;
	if(this.showed){
		document.write(s);
		this.copyToStyle(this.selectedIndex);
	}	
}
PasswordStrength.prototype.copyToObject = function(o1,o2){
	for(var i in o1){
		o2[i] = o1[i];
	}
}
PasswordStrength.prototype.copyToStyle = function(id){
	this.selectedIndex = id;
	for(var i=0;i<3;i++){
		if(i == id-1){
			this.$(this.divName+"_label_"+i).style.display = "inline";
		}else{
			this.$(this.divName+"_label_"+i).style.display = "none";
		}
	}
	for(var i=0;i<id;i++){
		this.copyToObject(this.styles[id],this.$(this.divName+"_td_"+i).style);			
	}
	for(;i<3;i++){
		this.copyToObject(this.styles[0],this.$(this.divName+"_td_"+i).style);
	}
}
PasswordStrength.prototype.$ = function(s){
	return document.getElementById(s);
}
PasswordStrength.prototype.setSize = function(w,h){
	this.width = w;
	this.height = h;
}
PasswordStrength.prototype.setMinLength = function(n){
	if(isNaN(n)){
		return ;
	}
	n = Number(n);
	if(n>1){
		this.minLength = n;
	}
}
PasswordStrength.prototype.setStyles = function(){
	if(arguments.length == 0){
		return ;
	}
	for(var i=0;i<arguments.length && i < 4;i++){
		this.styles[i] = arguments[i];
	}
	this.copyToStyle(this.selectedIndex);
}
PasswordStrength.prototype.write = function(s){
	if(this.showed){
		return ;
	}
	var n = (s == 'string') ? this.$(s) : s;
	if(typeof(n) != "object"){
		return ;
	}
	n.innerHTML = this.content;
	this.copyToStyle(this.selectedIndex);
}
PasswordStrength.prototype.update = function(s){
	if(s.length < this.minLen){
		this.copyToStyle(0);
		return;
	}
	var ls = -1;
	if (s.match(/[a-z]/ig)){
		ls++;
	}
	if (s.match(/[0-9]/ig)){
		ls++;
	}
 	if (s.match(/(.[^a-z0-9])/ig)){
		ls++;
	}
	if (s.length < 6 && ls > 0){
		ls--;
	}
	 switch(ls) { 
		 case 0:
			 this.copyToStyle(1);
			 break;
		 case 1:
			 this.copyToStyle(2);
			 break;
		 case 2:
			 this.copyToStyle(3);
			 break;
		 default:
			 this.copyToStyle(0);
	 }
}