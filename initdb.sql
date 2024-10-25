--
-- PostgreSQL database dump
--

-- Dumped from database version 16.3
-- Dumped by pg_dump version 16.3

-- Started on 2024-10-25 10:52:33

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 4843 (class 1262 OID 24692)
-- Name: svhdb; Type: DATABASE; Schema: -; Owner: postgres
--

---CREATE DATABASE svhdb WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_United States.1252';
CREATE DATABASE my_db_name WITH  ENCODING 'UTF8' LC_COLLATE='English_United States.UTF8' LC_CTYPE='English_United States.UTF8' TEMPLATE=template0;

ALTER DATABASE svhdb OWNER TO postgres;

\connect svhdb

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 4829 (class 0 OID 24711)
-- Dependencies: 216
-- Data for Name: Users; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4828 (class 0 OID 24693)
-- Dependencies: 215
-- Data for Name: __EFMigrationsHistory; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20240918173112_InitUser', '8.0.8');
INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20240923200750_UpdateClientDb', '8.0.8');
INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20240929093224_UpdateClientDbAddHistory', '8.0.8');
INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20240929160106_UpdateClientDbAddTypeDoc', '8.0.8');
INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20241014165000_UpdateClientDbValueGenerated', '8.0.8');
INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20241016165718_UpdateClientDbTypeDocDate', '8.0.8');


--
-- TOC entry 4835 (class 0 OID 24751)
-- Dependencies: 222
-- Data for Name: documents; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4837 (class 0 OID 24782)
-- Dependencies: 224
-- Data for Name: history_pkg; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4833 (class 0 OID 24733)
-- Dependencies: 220
-- Data for Name: packages; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4831 (class 0 OID 24719)
-- Dependencies: 218
-- Data for Name: pkg_status; Type: TABLE DATA; Schema: public; Owner: postgres
--



--
-- TOC entry 4844 (class 0 OID 0)
-- Dependencies: 223
-- Name: documents_did_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.documents_did_seq', 1, false);


--
-- TOC entry 4845 (class 0 OID 0)
-- Dependencies: 221
-- Name: documents_size_doc_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.documents_size_doc_seq', 1, false);


--
-- TOC entry 4846 (class 0 OID 0)
-- Dependencies: 219
-- Name: packages_pid_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.packages_pid_seq', 1, false);


--
-- TOC entry 4847 (class 0 OID 0)
-- Dependencies: 217
-- Name: pkg_status_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.pkg_status_id_seq', 1, false);


-- Completed on 2024-10-25 10:52:33

--
-- PostgreSQL database dump complete
--

