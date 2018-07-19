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

		stage('Test') {
			steps {
				sh 'dotnet test Tests/ --configuration Release'
			}
		}
	}
}
