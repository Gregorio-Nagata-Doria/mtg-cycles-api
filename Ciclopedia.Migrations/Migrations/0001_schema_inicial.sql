CREATE EXTENSION IF NOT EXISTS unaccent;
CREATE TABLE sets (
  setCode     text     PRIMARY KEY,  
  setName       text     NOT NULL,   
  setSymbol  text              
);


CREATE TABLE ciclos (
  slug     text     PRIMARY KEY,  
  nome_en        text     NOT NULL,
   nome_pt        text   NOT NULL,
  set        text REFERENCES sets(setCode),
  year        integer     NULL,
  artCoherent       boolean NOT NULL,
  rarity       text  ,
  structure        text  ,
    colors        text     NOT NULL   
);


CREATE TABLE cartas (
  id     integer     PRIMARY KEY,  
ciclo        text   NOT NULL  REFERENCES ciclos(slug),
  posicao        integer     NOT NULL,
  nome_en         text     NOT NULL,
  nome_pt       text   ,
  set            text  NOT NULL REFERENCES sets(setCode),
  rarity       text     NOT NULL,
  cmc        integer     NOT NULL,
    typeLine        text     NOT NULL,
    typelinept      text,
    image        text     NOT NULL,   

    artCrop        text     NOT NULL,   

    scryfallId        text     NOT NULL,   

    releasedAt        date     NOT NULL,
collectorNumber text  NOT NULL
);


CREATE TABLE carta_cor (
   carta_id      integer     REFERENCES cartas(id),  
cor        text  CHECK (cor IN ('W','U','R','B','G')),
 PRIMARY KEY (carta_id, cor)
);


