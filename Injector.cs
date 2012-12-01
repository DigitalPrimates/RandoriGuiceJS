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

using System;
using SharpKit.JavaScript;
using guice.binding;
using guice.reflection;
using guice.resolver;

namespace guice {

    [JsType(JsMode.Prototype, OmitCasts = true, NativeOverloads = false)]
    public class Injector {
        readonly protected Binder binder;
        readonly protected ClassResolver classResolver;

        public object getInstance( Type dependency ) {
            return resolveDependency( new TypeDefinition(dependency) );
        }

        public object getInstance(TypeDefinition dependencyTypeDefinition) {
            return resolveDependency(dependencyTypeDefinition);
        }

        internal virtual AbstractBinding getBinding( TypeDefinition typeDefinition ) {
            return binder.getBinding( typeDefinition );
        }

        internal void configureBinder( GuiceModule module ) {
            if ( module != null ) {
                module.configure( binder );
            }
        }

        //Entry point for TypeAbstractBinding to ask for a class.... 
        //This method does so without trying to resolve the class first, which is important if we are called from within a resolution
        public object buildClass(TypeDefinition typeDefinition) {
            JsArray<InjectionPoint> constructorPoints;
            JsArray<InjectionPoint> fieldPoints;
            object instance;

            constructorPoints = typeDefinition.getConstructorParameters();
            instance = buildFromInjectionInfo(typeDefinition, constructorPoints);

            fieldPoints = typeDefinition.getInjectionFields();
            injectMembersFromInjectionInfo(instance, fieldPoints);
            //injectMembersMethods( built, type );

            return instance;
        }

        public void injectMembers(dynamic instance) {
            Type constructor = instance.constructor;

            TypeDefinition dependency = new TypeDefinition( constructor );
            JsArray<InjectionPoint> fieldPoints;

            fieldPoints = dependency.getInjectionFields();
            injectMembersFromInjectionInfo(instance, fieldPoints);
        }

        object buildFromInjectionInfo(TypeDefinition dependencyTypeDefinition, JsArray<InjectionPoint> constructorPoints) {
            JsArray<object> args = new JsArray<object>();

            for (int i = 0; i < constructorPoints.length; i++) {
                args[i] = resolveDependency(classResolver.resolveClassName(constructorPoints[i].t));
            }

            object obj = dependencyTypeDefinition.constructorApply(args);
            return obj;
        }

        void injectMembersFromInjectionInfo(object instance, JsArray<InjectionPoint> fieldPoints) {
            JsObject instanceMap = instance.As<JsObject>();

            for (int i = 0; i < fieldPoints.length; i++) {
                instanceMap[fieldPoints[i].n] = resolveDependency(classResolver.resolveClassName(fieldPoints[i].t));
            }
        }

        object resolveDependency(TypeDefinition typeDefinition) {
            AbstractBinding abstractBinding = getBinding(typeDefinition);
            object instance;

            if (abstractBinding != null) {
                instance = abstractBinding.provide(this);
            } else {
                instance = buildClass(typeDefinition);
            }

            return instance;
        }

        public Injector(Binder binder, ClassResolver classResolver) {
            this.binder = binder;
            this.classResolver = classResolver;
        }
    }
}