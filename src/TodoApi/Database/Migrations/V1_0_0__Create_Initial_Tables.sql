CREATE TABLE public.persons (
    userid uuid NOT NULL PRIMARY KEY,
    fullname character varying NOT NULL,
    updated timestamp NOT NULL DEFAULT NOW(),
    created timestamp NOT NULL DEFAULT NOW()
);

