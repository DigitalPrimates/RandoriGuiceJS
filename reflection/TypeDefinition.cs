/***
 * Copyright 2012 LTN Consulting, Inc. /dba Digital Primates®
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 * 
 * @author Michael Labriola <labriola@digitalprimates.net>
 */
using SharpKit.JavaScript;

namespace guice.reflection {

    [JsType(JsMode.Json)]
    public enum InjectionTypes { Constructor, Property, Method, View };

    [JsType(JsMode.Prototype, OmitCasts = true, Export = false, Name = "Object")]
    public class InjectionPoint {
        public JsString n; //name
        public JsString t; //type as string
        public int r; //is it required (1 or 0, we only care if it says 0)
        public dynamic v; //value, if it was given a default, only considered when r=0
    }

    public class TypeDefinition {

        readonly dynamic _type;

        public dynamic type { 
            get { return _type; }
        }

        private JsArray<InjectionPoint> injectionPoints(InjectionTypes injectionType) {
            return this._type.injectionPoints(injectionType);
        }

        public JsString getClassName() {
            JsString className = _type.className;

            if (className == JsContext.undefined ) {
                throw new JsError("Class not does defined a usable className");
            }

            return className;
        }

        public JsString getSuperClassName() {
            JsString className = _type.superClassName;

            if (className == JsContext.undefined) {
                className = "Object";
            }

            return className;
        }

        public JsArray<InjectionPoint> getInjectionFields() {
            return injectionPoints( InjectionTypes.Property );
        }

        public JsArray<InjectionPoint> getViewFields() {
            return injectionPoints(InjectionTypes.View);
        }

        public JsArray<InjectionPoint> getConstructorParameters() {
            return injectionPoints(InjectionTypes.Constructor);
        }

        public object constructorApply(JsArray<object> args) {

            object instance = null;

            JsContext.JsCode("void('#RANDORI_IGNORE_BEGIN')");
            JsContext.JsCode(@" 
    var f, c;
    c = this._type; // reference to class constructor function
    f = function(){}; // dummy function
    f.prototype = c.prototype; // reference same prototype
    instance = new f(); // instantiate dummy function to copy prototype properties
    c.apply(instance, args); // call class constructor, supplying new object as context
    instance.constructor = c; // assign correct constructor (not f)
");
            JsContext.JsCode("void('#RANDORI_IGNORE_END')");

            return instance;
        }

        public TypeDefinition( dynamic type ) {
            if (type == null) {
                throw new JsError("Cannot build class injection of primitives not supported at this time ");
            }

            this._type = type;
        }
    }
}