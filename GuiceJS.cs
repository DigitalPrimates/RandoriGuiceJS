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

using SharpKit.Html;
using SharpKit.JavaScript;
using guice.binding;
using guice.loader;
using guice.resolvers;

namespace guice {

    public abstract class GuiceModule {
        public abstract void configure(Binder binder);
    }

    public class GuiceJs {

        public Injector createInjector(GuiceModule module) {
            var hashMap = new BindingHashMap();
            var binder = new Binder( hashMap );
            var loader = new SynchronousClassLoader(new XMLHttpRequest(), "generated/");
            var classResolver = new ClassResolver( loader );

            if (module != null) {
                module.configure(binder);
            }

            var injector = new Injector(binder, classResolver);
            binder.bind(typeof(Injector)).toInstance(injector);
            binder.bind(typeof(ClassResolver)).toInstance(classResolver);
            binder.bind(typeof(SynchronousClassLoader)).toInstance(loader);

            return injector;
        }

        //This is a little evil and I am not sure I like it, but it is the best way we can provide bindings to a child injector for now.
        public void configureInjector(Injector injector, GuiceModule module) {
            injector.configureBinder( module );
        }

    }
}