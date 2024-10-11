-- Postgres init script

-- Create the table
CREATE TABLE IF NOT EXISTS public.weatherstations
(
    id character varying(5) PRIMARY KEY,
    name character varying(50) NOT NULL,
    timezone character varying(50) NOT NULL,
    country character varying(2) NOT NULL,
    latitude double precision NOT NULL,
    longitude double precision NOT NULL,
    elevation integer NOT NULL
);

-- Insert some sample data
-- Thank you https://github.com/meteostat/weather-stations
INSERT INTO weatherstations (id, name, timezone, country, latitude, longitude, elevation) VALUES
('00FAY','Holden Agdm','America/Edmonton','CA',53.19,-112.25,688),
('00TG6','Athabasca 1','America/Edmonton','CA',54.72,-113.29,515),
('01001','Jan Mayen','Europe/Oslo','NO',70.9333,-8.6667,10),
('01002','Grahuken','Europe/Oslo','NO',79.7833,14.4667,0),
('01003','Hornsund','Europe/Oslo','NO',77,15.5,10),
('01004','New Alesund II','Europe/Oslo','NO',78.9167,11.9333,8),
('01005','Barentsburg','Arctic/Longyearbyen','NO',78.0667,13.6333,9),
('01006','Edgeoya','Europe/Oslo','NO',78.2333,22.7833,0),
('01007','New Alesund','Europe/Oslo','NO',78.9167,11.9333,0),
('01008','Svalbard Lufthavn','Europe/Oslo','NO',78.25,15.4667,2),
('01015','Hekkingen Lighthouse','Europe/Oslo','NO',69.6,17.8333,14),
('01016','Senja-Grasmyrskogen','Europe/Oslo','NO',69.2833,17.7667,50),
('01024','Lanes','Europe/Oslo','NO',78.6556,16.3603,20),
('01025','Tromso Airport','Europe/Oslo','NO',69.6833,18.9167,1),
('01026','Tromso','Europe/Oslo','NO',69.65,18.9333,103),
('01027','Tromso-holt / Åsgård','Europe/Oslo','NO',69.6522,18.9056,10),
('01028','Bjornoya','Europe/Oslo','NO',74.5167,19.0167,16),
('01033','Torsvag Lighthouse','Europe/Oslo','NO',70.25,19.5,21),
('01037','Skibotn / Skibotin','Europe/Oslo','NO',69.3861,20.2594,5),
('01045','Nordstraum I Kvaenangen','Europe/Oslo','NO',69.8333,21.8833,6),
('01046','Sorkjosen','Europe/Oslo','NO',69.7833,20.9667,5),
('01047','Kautokeino','Europe/Oslo','NO',69,23.0333,305),
('01049','Alta Airport','Europe/Oslo','NO',69.9833,23.3667,3),
('01051','Suolovuopmi','Europe/Oslo','NO',69.5833,23.5333,374),
('01055','Fruholmen Lighthouse','Europe/Oslo','NO',71.1,24,13),
('01057','Cuovddatmohkki','Europe/Oslo','NO',69.3667,24.4333,286),
('01058','Suolovuopmi Lulit / Masi','Europe/Oslo','NO',69.5794,23.5344,381),
('01059','Banak','Europe/Oslo','NO',70.0667,24.9833,0),
('01062','Hopen','Europe/Oslo','NO',76.5,25.0667,6),
('01065','Karasjok','Europe/Oslo','NO',69.4667,25.5,129),
('01068','Honningsvag Airport','Europe/Oslo','NO',71.0167,25.9833,1)
ON CONFLICT DO NOTHING;
