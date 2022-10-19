/* ##################### INSERTS  ######################## */
INSERT INTO ACCESS_LEVEL (NAME) VALUES ('G1'), ('G2'), ('G3');
INSERT INTO SHIFT (NAME, CLOCK_IN, CLOCK_OUT, DAY_COUNT, LUNCH_TIME) VALUES 
	('Morning Shift', maketime(8, 00, 00), maketime(18, 00, 00), 0, maketime(12, 00, 00)), 
	('Evening Shift', maketime(16, 00, 00), maketime(2, 00, 00), 1, maketime(20, 00, 00));
    
/*INSERT INTO DEVICE (NAME,  CREATED_BY, ACCESS_LEVEL_ID) VALUES 
('Office', 'API_CTL', 2), ('Entrance A', 'API_CTL', 4), ('Entrance B', 'API_CTL', 3), ('Warehouse', 'API_CTL', 1);*/
