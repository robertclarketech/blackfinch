# Blackfinch Technical Test Frontend

## Setup

Please ensure you have the following installed:

- NodeJS 22 or later, including npm
- `openapi-generator-cli` (only required for regenerating the client code from swagger)

If you use nix, a `flake.nix` and `.envrc` is provided. Run `.direnv allow` to have a developer environment setup for you.

Once you have npm, run `npm i` to install node dependencies.

The application requires the following environment variables, defaults have been placed in `.env.development`:

- `VITE_BLACKFINCH_BASE_URL`: The base url for the API

## Using OpenAPI Generator

The client code for communicating to the API is automatically generated from the APIs swaagger. If you need to regenerate this code, run `npm run openapi-gen`

## Running a development build

Run `npm run dev` to run a development build.

## Creating a production build

Run `npm run build` to run a production build. This will perform full typechecking via tsc and a full lint using biome; if either of these fail then the build will abort.
Once the build has finished, a folder called dist will appear on the route directory with the production assets

## Not implemented

Integration Tests
