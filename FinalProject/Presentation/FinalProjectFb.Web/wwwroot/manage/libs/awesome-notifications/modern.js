!function(e,t){"object"==typeof exports&&"object"==typeof module?module.exports=t():"function"==typeof define&&define.amd?define([],t):"object"==typeof exports?exports.AWN=t():e.AWN=t()}(self,(function(){return(()=>{"use strict";var e={d:(t,s)=>{for(var n in s)e.o(s,n)&&!e.o(t,n)&&Object.defineProperty(t,n,{enumerable:!0,get:s[n]})},o:(e,t)=>Object.prototype.hasOwnProperty.call(e,t),r:e=>{"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(e,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(e,"__esModule",{value:!0})}},t={};e.r(t),e.d(t,{default:()=>f});const s={maxNotifications:10,animationDuration:300,position:"bottom-right",labels:{tip:"Tip",info:"Info",success:"Success",warning:"Attention",alert:"Error",async:"Loading",confirm:"Confirmation required",confirmOk:"OK",confirmCancel:"Cancel"},icons:{tip:"question-circle",info:"info-circle",success:"check-circle",warning:"exclamation-circle",alert:"exclamation-triangle",async:"cog fa-spin",confirm:"exclamation-triangle",prefix:"<i class='fa fas fa-fw fa-",suffix:"'></i>",enabled:!0},replacements:{tip:null,info:null,success:null,warning:null,alert:null,async:null,"async-block":null,modal:null,confirm:null,general:{"<script>":"","<\/script>":""}},messages:{tip:"",info:"",success:"Action has been succeeded",warning:"",alert:"Action has been failed",confirm:"This action can't be undone. Continue?",async:"Please, wait...","async-block":"Loading"},formatError(e){if(e.response){if(!e.response.data)return"500 API Server Error";if(e.response.data.errors)return e.response.data.errors.map((e=>e.detail)).join("<br>");if(e.response.statusText)return`${e.response.status} ${e.response.statusText}: ${e.response.data}`}return e.message?e.message:e},durations:{global:5e3,success:null,info:null,tip:null,warning:null,alert:null},minDurations:{async:1e3,"async-block":1e3}};class n{constructor(e={},t=s){Object.assign(this,this.defaultsDeep(t,e))}icon(e){return this.icons.enabled?`${this.icons.prefix}${this.icons[e]}${this.icons.suffix}`:""}label(e){return this.labels[e]}duration(e){let t=this.durations[e];return null===t?this.durations.global:t}toSecs(e){return e/1e3+"s"}applyReplacements(e,t){if(!e)return this.messages[t]||"";for(const s of["general",t])if(this.replacements[s])for(const t in this.replacements[s])e=e.replace(t,this.replacements[s][t]);return e}override(e){return e?new n(e,this):this}defaultsDeep(e,t){let s={};for(const n in e)t.hasOwnProperty(n)?s[n]="object"==typeof e[n]&&null!==e[n]?this.defaultsDeep(e[n],t[n]):t[n]:s[n]=e[n];return s}}const i="awn-popup",r="awn-toast",a="awn-btn",o="awn-confirm",l={prefix:r,klass:{label:`${r}-label`,content:`${r}-content`,icon:`${r}-icon`,progressBar:`${r}-progress-bar`,progressBarPause:`${r}-progress-bar-paused`},ids:{container:`${r}-container`}},c={prefix:i,klass:{buttons:"awn-buttons",button:a,successBtn:`${a}-success`,cancelBtn:`${a}-cancel`,title:`${i}-title`,body:`${i}-body`,content:`${i}-content`,dotAnimation:`${i}-loading-dots`},ids:{wrapper:`${i}-wrapper`,confirmOk:`${o}-ok`,confirmCancel:`${o}-cancel`}},d={hiding:"awn-hiding"},h=class{constructor(e,t,s,n,i){this.newNode=document.createElement("div"),t&&(this.newNode.id=t),s&&(this.newNode.className=s),n&&(this.newNode.style.cssText=n),this.parent=e,this.options=i}beforeInsert(){}afterInsert(){}insert(){return this.beforeInsert(),this.el=this.parent.appendChild(this.newNode),this.afterInsert(),this}replace(e){if(this.getElement())return this.beforeDelete().then((()=>(this.updateType(e.type),this.parent.replaceChild(e.newNode,this.el),this.el=this.getElement(e.newNode),this.afterInsert(),this)))}beforeDelete(e=this.el){let t=0;return this.start&&(t=this.options.minDurations[this.type]+this.start-Date.now(),t<0&&(t=0)),new Promise((s=>{setTimeout((()=>{e.classList.add(d.hiding),setTimeout(s,this.options.animationDuration)}),t)}))}delete(e=this.el){return this.getElement(e)?this.beforeDelete(e).then((()=>{e.remove(),this.afterDelete()})):null}afterDelete(){}getElement(e=this.el){return e?document.getElementById(e.id):null}addEvent(e,t){this.el.addEventListener(e,t)}toggleClass(e){this.el.classList.toggle(e)}updateType(e){this.type=e,this.duration=this.options.duration(this.type)}},u=class extends h{constructor(e,t,s,n){super(n,`${l.prefix}-${Math.floor(Date.now()-100*Math.random())}`,`${l.prefix} ${l.prefix}-${t}`,`animation-duration: ${s.toSecs(s.animationDuration)};`,s),this.updateType(t),this.setInnerHtml(e)}setInnerHtml(e){"alert"===this.type&&e&&(e=this.options.formatError(e)),e=this.options.applyReplacements(e,this.type),this.newNode.innerHTML=`<div class="awn-toast-wrapper">${this.progressBar}${this.label}<div class="${l.klass.content}">${e}</div><span class="${l.klass.icon}">${this.options.icon(this.type)}</span></div>`}beforeInsert(){if(this.parent.childElementCount>=this.options.maxNotifications){let e=Array.from(this.parent.getElementsByClassName(l.prefix));this.delete(e.find((e=>!this.isDeleted(e))))}}afterInsert(){if("async"==this.type)return this.start=Date.now();if(this.addEvent("click",(()=>this.delete())),!(this.duration<=0)){this.timer=new class{constructor(e,t){this.callback=e,this.remaining=t,this.resume()}pause(){this.paused=!0,window.clearTimeout(this.timerId),this.remaining-=new Date-this.start}resume(){this.paused=!1,this.start=new Date,window.clearTimeout(this.timerId),this.timerId=window.setTimeout((()=>{window.clearTimeout(this.timerId),this.callback()}),this.remaining)}toggle(){this.paused?this.resume():this.pause()}}((()=>this.delete()),this.duration);for(const e of["mouseenter","mouseleave"])this.addEvent(e,(()=>{this.isDeleted()||(this.toggleClass(l.klass.progressBarPause),this.timer.toggle())}))}}isDeleted(e=this.el){return e.classList.contains(d.hiding)}get progressBar(){return this.duration<=0||"async"===this.type?"":`<div class='${l.klass.progressBar}' style="animation-duration:${this.options.toSecs(this.duration)};"></div>`}get label(){return`<b class="${l.klass.label}">${this.options.label(this.type)}</b>`}},p=class extends h{constructor(e,t="modal",s,n,i){let r=`animation-duration: ${s.toSecs(s.animationDuration)};`;super(document.body,c.ids.wrapper,null,r,s),this[c.ids.confirmOk]=n,this[c.ids.confirmCancel]=i,this.className=`${c.prefix}-${t}`,["confirm","async-block","modal"].includes(t)||(t="modal"),this.updateType(t),this.setInnerHtml(e),this.insert()}setInnerHtml(e){let t=this.options.applyReplacements(e,this.type);switch(this.type){case"confirm":let e=[`<button class='${c.klass.button} ${c.klass.successBtn}'id='${c.ids.confirmOk}'>${this.options.labels.confirmOk}</button>`];!1!==this[c.ids.confirmCancel]&&e.push(`<button class='${c.klass.button} ${c.klass.cancelBtn}'id='${c.ids.confirmCancel}'>${this.options.labels.confirmCancel}</button>`),t=`${this.options.icon(this.type)}<div class='${c.klass.title}'>${this.options.label(this.type)}</div><div class="${c.klass.content}">${t}</div><div class='${c.klass.buttons} ${c.klass.buttons}-${e.length}'>${e.join("")}</div>`;break;case"async-block":t=`${t}<div class="${c.klass.dotAnimation}"></div>`}this.newNode.innerHTML=`<div class="${c.klass.body} ${this.className}">${t}</div>`}keyupListener(e){if("async-block"===this.type)return e.preventDefault();switch(e.code){case"Escape":e.preventDefault(),this.delete();case"Tab":if(e.preventDefault(),"confirm"!==this.type||!1===this[c.ids.confirmCancel])return!0;let t=this.okBtn;e.shiftKey?document.activeElement.id==c.ids.confirmOk&&(t=this.cancelBtn):document.activeElement.id!==c.ids.confirmCancel&&(t=this.cancelBtn),t.focus()}}afterInsert(){switch(this.listener=e=>this.keyupListener(e),window.addEventListener("keydown",this.listener),this.type){case"async-block":this.start=Date.now();break;case"confirm":this.okBtn.focus(),this.addEvent("click",(e=>{if("BUTTON"!==e.target.nodeName)return!1;this.delete(),this[e.target.id]&&this[e.target.id]()}));break;default:document.activeElement.blur(),this.addEvent("click",(e=>{e.target.id===this.newNode.id&&this.delete()}))}}afterDelete(){window.removeEventListener("keydown",this.listener)}get okBtn(){return document.getElementById(c.ids.confirmOk)}get cancelBtn(){return document.getElementById(c.ids.confirmCancel)}};class f{constructor(e={}){this.options=new n(e)}tip(e,t){return this._addToast(e,"tip",t).el}info(e,t){return this._addToast(e,"info",t).el}success(e,t){return this._addToast(e,"success",t).el}warning(e,t){return this._addToast(e,"warning",t).el}alert(e,t){return this._addToast(e,"alert",t).el}async(e,t,s,n,i){let r=this._addToast(n,"async",i);return this._afterAsync(e,t,s,i,r)}confirm(e,t,s,n){return this._addPopup(e,"confirm",n,t,s)}asyncBlock(e,t,s,n,i){let r=this._addPopup(n,"async-block",i);return this._afterAsync(e,t,s,i,r)}modal(e,t,s){return this._addPopup(e,t,s)}closeToasts(){let e=this.container;for(;e.firstChild;)e.removeChild(e.firstChild)}_addPopup(e,t,s,n,i){return new p(e,t,this.options.override(s),n,i)}_addToast(e,t,s,n){s=this.options.override(s);let i=new u(e,t,s,this.container);return n?n instanceof p?n.delete().then((()=>i.insert())):n.replace(i):i.insert()}_afterAsync(e,t,s,n,i){return e.then(this._responseHandler(t,"success",n,i),this._responseHandler(s,"alert",n,i))}_responseHandler(e,t,s,n){return i=>{switch(typeof e){case"undefined":case"string":let r="alert"===t?e||i:e;this._addToast(r,t,s,n);break;default:n.delete().then((()=>{e&&e(i)}))}}}_createContainer(){return new h(document.body,l.ids.container,`awn-${this.options.position}`).insert().el}get container(){return document.getElementById(l.ids.container)||this._createContainer()}}return t})()}));