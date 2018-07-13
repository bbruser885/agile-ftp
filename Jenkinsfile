#!/usr/bin/env groovy
// Declarative //
pipeline {
	agent any

	stages {
		stage ('Build') {
			steps {
				sh 'dotnet build --configuration Release'
			}
		}
/* uncomment when we have tests
		stage('Test') {
			steps {
				sh 'dotnet test --configuration Release'
			}
		}
*/
	}
}
