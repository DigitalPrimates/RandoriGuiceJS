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
using guice.reflection;
using guice.resolvers;

namespace guice {
    [JsType(JsMode.Prototype, OmitCasts = true, NativeOverloads = false)]
    public class InjectionClassBuilder {

        readonly Injector injector;
        readonly ClassResolver classResolver;

        public object buildClass( JsString className ) {
            TypeDefinition type = classResolver.resolveClassName(className);

            return injector.getInstance(type);
        }

        [JsMethod(NativeParams=false)]
        public object buildClass(JsString className, params object[] list) {
            object instance;
            TypeDefinition typeDefinition = classResolver.resolveClassName( className );

            //this feels like it needs to be refactored
            instance = typeDefinition.constructorApply(list);
            injector.injectMembers(instance);
            
            return instance;
        }

        public InjectionClassBuilder(Injector injector, ClassResolver classResolver=null) {
            this.injector = injector;
            this.classResolver = classResolver;
        }
    }
}