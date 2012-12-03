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

        //Entry point for TypeAbstractBinding to ask for a class.... 
        //This method does so without trying to resolve the class first, which is important if we are called from within a resolution
        public object buildClass(TypeDefinition typeDefinition) {

            var constructorPoints = typeDefinition.getConstructorParameters();
            var instance = buildFromInjectionInfo(typeDefinition, constructorPoints);

            var fieldPoints = typeDefinition.getInjectionFields();
            injectMemberPropertiesFromInjectionInfo(instance, fieldPoints);

            var methodPoints = typeDefinition.getInjectionMethods();
            injectMembersMethodsFromInjectionInfo(instance, methodPoints);

            return instance;
        }

        public void injectMembers(dynamic instance) {
            Type constructor = instance.constructor;

            var typeDefinition = new TypeDefinition(constructor);

            var fieldPoints = typeDefinition.getInjectionFields();
            injectMemberPropertiesFromInjectionInfo(instance, fieldPoints);

            var methodPoints = typeDefinition.getInjectionMethods();
            injectMembersMethodsFromInjectionInfo(instance, methodPoints);
        }

        object buildFromInjectionInfo(TypeDefinition dependencyTypeDefinition, JsArray<InjectionPoint> constructorPoints) {
            var args = new JsArray<object>();

            for (int i = 0; i < constructorPoints.length; i++) {
                args[i] = resolveDependency(classResolver.resolveClassName(constructorPoints[i].t));
            }

            object obj = dependencyTypeDefinition.constructorApply(args);
            return obj;
        }

        void injectMemberPropertiesFromInjectionInfo(object instance, JsArray<InjectionPoint> fieldPoints) {
            var instanceMap = instance.As<JsObject>();

            for (var i = 0; i < fieldPoints.length; i++) {
                instanceMap[fieldPoints[i].n] = resolveDependency(classResolver.resolveClassName(fieldPoints[i].t));
            }
        }

//p.push({n:'builder', t:'behavior.EchoBehavior', p:'[{n:'builder', t:'guice.InjectionClassBuilder'}']});
//p.push({n:'builderPlus', t:'behavior.EchoBehavior', p:'[{n:'builder', t:'guice.InjectionClassBuilder'}, {n:'resolver', t:'guice.resolver.ClassResolver'}']});

        void injectMembersMethodsFromInjectionInfo(object instance, JsArray<MethodInjectionPoint> methodPoints) {
            var instanceMap = instance.As<JsObject>();

            for (var i = 0; i < methodPoints.length; i++) {
                var methodPoint = methodPoints[i];
                var args = new JsArray<object>();

                for (var j = 0; j < methodPoint.p.length; j++) {
                    var parameterPoint = methodPoint.p[j];
                    args[j] = resolveDependency(classResolver.resolveClassName(parameterPoint.t));
                }

                var action = instanceMap[methodPoints[i].n].As<JsFunction>();
                action.apply(instance, args);
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