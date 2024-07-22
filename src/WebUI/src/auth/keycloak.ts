import { AuthClientError, AuthClientEvent } from '@react-keycloak/core';
import { kcConfig } from 'configurations';
import Keycloak, { KeycloakConfig } from 'keycloak-js';

const initOptions: KeycloakConfig = {
    url: kcConfig.url,
    realm: kcConfig.realm,
    clientId: kcConfig.clientId,
}

export const keycloakOptions = {
    onLoad: 'login-required',
}

interface KeycloakTokens {
    token?: string;
    refreshToken?: string;
    idToken?: string;
}

export const onKeycloakEvent = (event: AuthClientEvent, error?: AuthClientError) => {
    console.log('onKeycloakEvent', event, error);
}

export const onKeycloakTokens = (tokens: KeycloakTokens) => {
    console.log('onKeycloakTokens', tokens);
}

export const kc = new Keycloak(initOptions)
