#!/usr/bin/env groovy
// Declarative //
pipeline {
	agent any

	stages {
		stage ('Build') {
			steps {
				sh 'dotnet build AgileFTP/ --configuration Release'
			}
		}

		stage('Test') {
			steps {
				script {
					try {
						sh 'dotnet test Tests/ --configuration Release'
					}
					catch (ex) {
						currentBuild.result = 'UNSTABLE'
					}
				}
			}
		}
	}
}
