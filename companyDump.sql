-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 15, 2025 at 04:18 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.4.1

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `companydump`
--

DELIMITER $$
--
-- Procedures
--
CREATE DEFINER=`root`@`localhost` PROCEDURE `full_name` (`emp_id` INT)   BEGIN
    -- Query to return the full name of the specific employee based on employee_id
    SELECT CONCAT(first_name, ' ', last_name) AS full_name
    FROM employees
    WHERE employee_id = emp_id;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `get_department_employee_count` ()   BEGIN
	 DECLARE done INT DEFAULT 0;
    DECLARE dept_id INT;
    DECLARE employee_count INT;
    
    -- Declare cursor to fetch department data
    DECLARE dept_cursor CURSOR FOR 
        SELECT department_id FROM departments;
    
    -- Declare CONTINUE handler for cursor completion
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;
    
    OPEN dept_cursor;
    
    read_loop: LOOP
        -- Fetch department data
        FETCH dept_cursor INTO dept_id;
        
        IF done THEN
            LEAVE read_loop;
        END IF;
        
        -- Count number of employees in the department
        SELECT COUNT(*) AS EmployeeCount FROM employees WHERE department_id = dept_id;
    END LOOP;
    
    CLOSE dept_cursor;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `get_employee_bonus_info` ()   BEGIN
	  DECLARE done INT DEFAULT 0;
    DECLARE emp_id INT;
    DECLARE emp_salary DECIMAL(10, 2);
    DECLARE emp_bonus DECIMAL(10, 2);
    
    -- Declare cursor to fetch employee data
    DECLARE emp_cursor CURSOR FOR 
        SELECT employee_id, salary FROM employees;
    
    -- Declare CONTINUE handler for cursor completion
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1;
    
    OPEN emp_cursor;
    
    read_loop: LOOP
        -- Fetch employee data from the cursor
        FETCH emp_cursor INTO emp_id, emp_salary;
        
        IF done THEN
            LEAVE read_loop;
        END IF;
        
        -- Check salary and calculate bonus
        IF emp_salary IS NULL OR emp_salary = 0 THEN
            SELECT 'Employee ID' AS Error, emp_id AS EmployeeID, 'Salary is NULL or 0' AS Message;
        ELSE
            -- Calculate bonus (e.g., 5% of the salary)
            SET emp_bonus = emp_salary * 0.05;
            SELECT emp_id AS EmployeeID, emp_salary AS Salary, emp_bonus AS BonusAmount;
        END IF;
    END LOOP;
    
    CLOSE emp_cursor;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `get_full_name` (`emp_id` INT)   BEGIN
    -- Query to return the full name of the specific employee based on employee_id
    SELECT CONCAT(first_name, ' ', last_name) AS full_name
    FROM employees
    WHERE employee_id = emp_id;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `get_full_names` (`employee_id` INT)   BEGIN
    -- Query to return full names for all employees with the given employee_id
    SELECT CONCAT(first_name, ' ', last_name) AS full_name
    FROM employees
    WHERE employee_id = employee_id;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `get_total_salary_by_department` ()   BEGIN
	-- Query to get the total salary by department
    SELECT d.department_name, SUM(e.salary) AS TotalSalary
    FROM employees e
    JOIN departments d ON e.department_id = d.department_id
    GROUP BY e.department_id;
END$$

CREATE DEFINER=`root`@`localhost` PROCEDURE `insert_new_employee` ()   BEGIN
	 -- Insert new employee data into the employees table
    INSERT INTO employees (first_name, last_name, email, salary, department_id) 
    VALUES (new_first_name, new_last_name, new_email, new_salary, new_department_id);
    
    -- Return a success message
    SELECT 'New employee inserted successfully' AS SuccessMessage;
END$$

--
-- Functions
--
CREATE DEFINER=`root`@`localhost` FUNCTION `calculate_bonus` (`salary` DECIMAL(10,2)) RETURNS DECIMAL(10,2) DETERMINISTIC BEGIN
    DECLARE bonus DECIMAL(10, 2);
    
    IF salary IS NULL OR salary = 0 THEN
        RETURN 0; -- No bonus if salary is NULL or 0
    ELSE
        SET bonus = salary * 0.05; -- 5% bonus
        RETURN bonus;
    END IF;
END$$

CREATE DEFINER=`root`@`localhost` FUNCTION `employee_email` (`emp_id` INT) RETURNS CHAR(100) CHARSET utf8mb4 COLLATE utf8mb4_general_ci DETERMINISTIC BEGIN
    DECLARE email CHAR(100);

    -- Query to fetch the email of the employee based on employee_id
    SELECT email INTO email
    FROM employees
    WHERE employee_id = emp_id;

    -- Return the email
    RETURN email;
END$$

CREATE DEFINER=`root`@`localhost` FUNCTION `employee_tenure` (`emp_id` INT) RETURNS INT(11) DETERMINISTIC BEGIN
    DECLARE tenure INT;
    -- Calculate the tenure by finding the difference in years between the current date and the hire date
    SELECT DATEDIFF(CURDATE(), hire_date) / 365 INTO tenure
    FROM employees
    WHERE employee_id = emp_id;  -- Use emp_id instead of employee_id for the parameter
    
    RETURN tenure;
END$$

CREATE DEFINER=`root`@`localhost` FUNCTION `full_name` (`employee_id` INT) RETURNS CHAR(100) CHARSET utf8mb4 COLLATE utf8mb4_general_ci DETERMINISTIC BEGIN
    DECLARE name CHAR(100);
    -- Use LIMIT 1 to ensure only one row is returned
    SELECT CONCAT(first_name, ' ', last_name) INTO name
    FROM employees
    WHERE employee_id = employee_id
    LIMIT 1;
    RETURN name;
END$$

CREATE DEFINER=`root`@`localhost` FUNCTION `get_full_name` (`emp_id` INT) RETURNS CHAR(100) CHARSET utf8mb4 COLLATE utf8mb4_general_ci DETERMINISTIC BEGIN
    DECLARE name CHAR(100);
    
    -- Fetch the full name of the employee with the given employee_id
    SELECT CONCAT(first_name, ' ', last_name) INTO name
    FROM employees
    WHERE employee_id = emp_id;

    -- Return the full name
    RETURN name;
END$$

CREATE DEFINER=`root`@`localhost` FUNCTION `total_salary` (`department_id` INT) RETURNS DOUBLE DETERMINISTIC BEGIN
    DECLARE total DOUBLE;
    -- Calculate the total salary (base salary + bonus + commission)
    SELECT SUM(s.base_salary + s.bonus + s.commission) INTO total
    FROM employees e
    JOIN salaries s ON e.employee_id = s.employee_id
    WHERE e.department_id = department_id;
    
    RETURN total;
END$$

CREATE DEFINER=`root`@`localhost` FUNCTION `total_salary_by_department` (`department_id` INT) RETURNS DECIMAL(10,2) DETERMINISTIC BEGIN
    DECLARE total_salary DECIMAL(10, 2);
    
    SELECT SUM(salary) INTO total_salary
    FROM employees
    WHERE department_id = department_id;
    
    RETURN total_salary;
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `audit_log`
--

CREATE TABLE `audit_log` (
  `id` int(11) NOT NULL,
  `action_type` varchar(50) DEFAULT NULL,
  `description` text DEFAULT NULL,
  `action_time` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `audit_log`
--

INSERT INTO `audit_log` (`id`, `action_type`, `description`, `action_time`) VALUES
(1, 'INSERT', 'New employee added: Alice Moral', '2025-04-22 10:22:54'),
(2, 'INSERT', 'New employee added: ALICE MORAL', '2025-04-22 10:25:34'),
(3, 'UPDATE', 'Employee updated: Alice Moral', '2025-04-22 10:31:28'),
(4, 'DELETE', 'Deleted employee: Alice Moral', '2025-04-22 10:44:57'),
(5, 'DELETE_ATTEMPT', 'Attempting to delete employee: Alice Moral', '2025-04-22 10:47:32'),
(6, 'DELETE', 'Deleted employee: Alice Moral', '2025-04-22 10:47:32'),
(7, 'ONE_TIME_EVENT', 'This is a one-time event', '2025-04-22 11:00:41'),
(8, 'RECURRING_EVENT', 'This is a recurring event', '2025-04-24 12:00:00'),
(9, 'INSERT', 'New employee added: TERRENZE BINAMIRA', '2025-04-27 14:41:41'),
(10, 'INSERT', 'New employee added: EREAR ASDA', '2025-04-27 14:42:10');

-- --------------------------------------------------------

--
-- Table structure for table `clients`
--

CREATE TABLE `clients` (
  `client_id` int(11) NOT NULL,
  `client_name` varchar(100) DEFAULT NULL,
  `contact_email` varchar(100) DEFAULT NULL,
  `contact_phone` varchar(15) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `clients`
--

INSERT INTO `clients` (`client_id`, `client_name`, `contact_email`, `contact_phone`) VALUES
(1, 'Tech Solutions', 'techsolutions@gmail.com', '09171234567'),
(2, 'Web Design Co.', 'webdesign@gmail.com', '09281234567'),
(3, 'FastTrack Logistics', 'asttrack@gmail.com', '09321234567'),
(4, 'Innovative Systems', 'innovativesys@gmail.com', '09431234567'),
(5, 'Global Enterprises', 'globalenterprises@gmail.com', '09541234567'),
(6, 'Quantum Tech', 'quantumtech@gmail.com', '09651234567'),
(7, 'Auto Parts Inc.', 'autoparts@gmail.com', '09761234567'),
(8, 'Creative Studios', 'creativestudios@gmail.com', '09871234567'),
(9, 'Cloud Solutions', 'loudsolutions@gmail.com', '09981234567'),
(10, 'Retail Hub', 'retailhub@gmail@gmail.com', '09182345678'),
(11, 'Data Insights', 'datainsights@gmail.com', '09292345678'),
(12, 'Market Leaders', 'marketleaders@gmail.com', '09302345678'),
(13, 'Health First', 'healthfirst@gmail.com', '09412345678'),
(14, 'Elite Ventures', 'eliteventures@gmail.com', '09522345678'),
(15, 'Tech Innovations', 'techinnovations@gmail.com', '09632345678');

-- --------------------------------------------------------

--
-- Stand-in structure for view `client_employee_overview`
-- (See below for the actual view)
--
CREATE TABLE `client_employee_overview` (
`client_id` int(11)
,`contact_email` varchar(100)
,`contact_phone` varchar(15)
,`employee_first_name` varchar(50)
,`employee_last_name` varchar(50)
,`employee_email` varchar(100)
);

-- --------------------------------------------------------

--
-- Table structure for table `departments`
--

CREATE TABLE `departments` (
  `department_id` int(11) NOT NULL,
  `department_name` varchar(100) DEFAULT NULL,
  `department_head` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `departments`
--

INSERT INTO `departments` (`department_id`, `department_name`, `department_head`) VALUES
(1, 'Human Resources', 1),
(2, 'Finance', 2),
(3, 'Marketing', 3),
(4, 'IT', 4),
(5, 'Operations', 5),
(6, 'Sales', 6),
(7, 'Legal', 7),
(8, 'R&D', 8),
(9, 'Customer Service', 9),
(10, 'Product Develoopment', 10),
(11, 'R&D', 11),
(12, 'Logistics', 12),
(13, 'Design', 13),
(14, 'Product Development', 14),
(15, 'Public Relations', 15);

-- --------------------------------------------------------

--
-- Stand-in structure for view `dept_employee_details`
-- (See below for the actual view)
--
CREATE TABLE `dept_employee_details` (
`department_id` int(11)
,`department_name` varchar(100)
,`first_name` varchar(50)
,`last_name` varchar(50)
,`email` varchar(100)
,`phone_number` varchar(15)
);

-- --------------------------------------------------------

--
-- Table structure for table `employees`
--

CREATE TABLE `employees` (
  `employee_id` int(11) NOT NULL,
  `first_name` varchar(50) DEFAULT NULL,
  `last_name` varchar(50) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `phone_number` varchar(15) DEFAULT NULL,
  `hire_date` date DEFAULT NULL,
  `department_id` int(11) DEFAULT NULL,
  `salary_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `employees`
--

INSERT INTO `employees` (`employee_id`, `first_name`, `last_name`, `email`, `phone_number`, `hire_date`, `department_id`, `salary_id`) VALUES
(1, 'Emmanuel', 'Martinez', 'emmanmartinez@gmail.com', '09171234567', '2023-04-16', 1, 1),
(2, 'Alice', 'Santos', 'alicesantos@gmail.com', '09282345678', '2023-02-20', 2, 2),
(3, 'Luis', 'Villanueva', 'luisvalenzuela@gmail.com', '09323456789', '2024-12-01', 3, 3),
(4, 'Jose', 'Mendonez', 'josemendonez@gmail.com', '09184567890', '2023-06-10', 4, 4),
(5, 'David', 'Jacinto', 'davidjacinto@gmail.com', '09565678901', '2022-10-05', 5, 5),
(6, 'Maricar', 'Dela Torre', 'maricardelatorre@gmail.com', '09476789012', '2025-01-25', 6, 6),
(7, 'Raymond', 'Villar', 'raymondvillar@gmail.com', '09167890123', '2022-09-15', 7, 7),
(8, 'Grace', 'Lladone', 'gracelladone@gmail.com', '09398901234', '2023-03-22', 8, 8),
(9, 'Helen', 'Cruz', 'helencruz@gmail.com', '09199012345\n\n', '2024-11-30', 9, 9),
(10, 'Isaac', 'Orlain', 'isaacorlain@gmail.com', '09230123456', '2024-03-10', 10, 10),
(11, 'Willie', 'Dantes', 'williedantes@gmail.com', '09431234567', '2023-07-14', 11, 11),
(12, 'Camilla', 'Bernardo', 'camillabernardo@gmail.com', '09312345678', '2022-04-18', 12, 12),
(13, 'Julio', 'Bonifacio', 'juliobonifacio@gmail.com', '09293456789', '2024-02-25', 13, 13),
(14, 'Jamill', 'Sy', 'jamillsy@gmail.com', '09534567890', '2024-08-08', 14, 14),
(15, 'Mae Ann', 'Villanueva', 'maevillanueva@gmail.com', '09155678901\n\n', '2025-04-02', 15, 15),
(19, 'TERRENZE', 'BINAMIRA', 'sekutsuko0204@gmail.com', '09936869633', '2025-04-27', NULL, NULL),
(20, 'EREAR', 'ASDA', 'asdas', '1231231', '2025-04-27', NULL, NULL);

--
-- Triggers `employees`
--
DELIMITER $$
CREATE TRIGGER `after_employee_delete` AFTER DELETE ON `employees` FOR EACH ROW BEGIN
    INSERT INTO audit_log (action_type, description, action_time)
    VALUES ('DELETE', CONCAT('Deleted employee: ', OLD.first_name, ' ', OLD.last_name), NOW());
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `after_employee_insert` AFTER INSERT ON `employees` FOR EACH ROW BEGIN
    INSERT INTO audit_log (action_type, description, action_time)
    VALUES ('INSERT', CONCAT('New employee added: ', NEW.first_name, ' ', NEW.last_name), NOW());
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `after_employee_update` AFTER UPDATE ON `employees` FOR EACH ROW BEGIN
    INSERT INTO audit_log (action_type, description, action_time)
    VALUES ('UPDATE', CONCAT('Employee updated: ', NEW.first_name, ' ', NEW.last_name), NOW());
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `before_employee_delete` BEFORE DELETE ON `employees` FOR EACH ROW BEGIN
    INSERT INTO audit_log (action_type, description, action_time)
    VALUES ('DELETE_ATTEMPT', CONCAT('Attempting to delete employee: ', OLD.first_name, ' ', OLD.last_name), NOW());
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `before_employee_insert` BEFORE INSERT ON `employees` FOR EACH ROW BEGIN
    SET NEW.first_name = UPPER(NEW.first_name);  -- Convert first name to uppercase
    SET NEW.last_name = UPPER(NEW.last_name);    -- Convert last name to uppercase
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `payments`
--

CREATE TABLE `payments` (
  `payment_id` int(11) NOT NULL,
  `employee_id` int(11) DEFAULT NULL,
  `payment_date` date DEFAULT NULL,
  `payment_amount` decimal(10,2) DEFAULT NULL,
  `payment_method` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `payments`
--

INSERT INTO `payments` (`payment_id`, `employee_id`, `payment_date`, `payment_amount`, `payment_method`) VALUES
(1, 1, '2025-06-10', NULL, 'Bank Transfer'),
(2, 2, '2025-08-10', NULL, 'Cash'),
(3, 3, '2025-06-25', NULL, 'Bank Transfer'),
(4, 4, '2025-05-10', NULL, 'Cheque'),
(5, 5, '2025-07-10', NULL, 'Bank Transfer'),
(6, 6, '2025-05-20', NULL, 'Cash'),
(7, 7, '2025-05-30', NULL, 'Cheque'),
(8, 8, '2025-06-30', NULL, 'Bank Transfer'),
(9, 9, '2025-07-10', NULL, 'Cash'),
(10, 10, '2025-05-10', NULL, 'Bank Transfer'),
(11, 11, '2025-07-10', NULL, 'Cheque'),
(12, 12, '2025-08-10', NULL, 'Bank Transfer'),
(13, 13, '2025-05-10', NULL, 'Cash'),
(14, 14, '2025-05-25', NULL, 'Cheque'),
(15, 15, '2025-06-20', NULL, 'Bank Transfer');

-- --------------------------------------------------------

--
-- Table structure for table `projects`
--

CREATE TABLE `projects` (
  `project_id` int(11) NOT NULL,
  `project_name` varchar(100) DEFAULT NULL,
  `client_id` int(11) DEFAULT NULL,
  `start_date` date DEFAULT NULL,
  `end_date` date DEFAULT NULL,
  `budget` decimal(10,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `projects`
--

INSERT INTO `projects` (`project_id`, `project_name`, `client_id`, `start_date`, `end_date`, `budget`) VALUES
(1, 'Website Redesign', 1, '2025-04-01', '2025-06-01', 100000.00),
(2, 'Mobile App Development', 2, '2025-03-10', '2025-08-01', 100000.00),
(3, 'New Product Launch', 3, '2025-01-15', '2025-06-15', 100000.00),
(4, 'System Upgrade', 4, '2025-02-01', '2025-04-30', 100000.00),
(5, 'Marketing Campaign', 5, '2025-04-01', '2025-06-30', 100000.00),
(6, 'Sales Expansion', 6, '2025-03-05', '2025-05-10', 100000.00),
(7, 'Employee Training', 7, '2025-04-10', '2025-05-20', 100000.00),
(8, 'Cloud Migration', 8, '2025-02-20', '2025-05-20', 100000.00),
(9, 'CRM Implementation', 9, '2025-03-01', '2025-07-01', 100000.00),
(10, 'Product Research', 10, '2025-01-05', '2025-04-30', 100000.00),
(11, 'Brand Revitalization', 11, '2025-03-10', '2025-07-01', 100000.00),
(12, 'E-commerce Platform', 12, '2025-04-02', '2025-08-01', 100000.00),
(13, 'Customer Portal Development', 13, '2025-01-30', '2025-04-30', 100000.00),
(14, 'Inventory Optimization', 14, '2025-02-15', '2025-05-15', 100000.00),
(15, 'Digital Marketing', 15, '2025-04-05', '2025-06-10', 100000.00);

-- --------------------------------------------------------

--
-- Table structure for table `salaries`
--

CREATE TABLE `salaries` (
  `salary_id` int(11) NOT NULL,
  `employee_id` int(11) DEFAULT NULL,
  `base_salary` decimal(10,2) DEFAULT NULL,
  `bonus` decimal(10,2) DEFAULT NULL,
  `commission` decimal(10,2) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `salaries`
--

INSERT INTO `salaries` (`salary_id`, `employee_id`, `base_salary`, `bonus`, `commission`) VALUES
(1, 1, 50000.00, 15000.00, 10000.00),
(2, 2, 50000.00, 15000.00, 10000.00),
(3, 3, 50000.00, 15000.00, 10000.00),
(4, 4, 50000.00, 15000.00, 10000.00),
(5, 5, 50000.00, 15000.00, 10000.00),
(6, 6, 50000.00, 15000.00, 10000.00),
(7, 7, 50000.00, 15000.00, 10000.00),
(8, 8, 50000.00, 15000.00, 10000.00),
(9, 9, 50000.00, 15000.00, 10000.00),
(10, 10, 50000.00, 15000.00, 10000.00),
(11, 11, 50000.00, 15000.00, 10000.00),
(12, 12, 50000.00, 15000.00, 10000.00),
(13, 13, 50000.00, 15000.00, 10000.00),
(14, 14, 50000.00, 15000.00, 10000.00),
(15, 15, 50000.00, 15000.00, 10000.00);

--
-- Triggers `salaries`
--
DELIMITER $$
CREATE TRIGGER `before_salary_update` BEFORE UPDATE ON `salaries` FOR EACH ROW BEGIN
    IF NEW.base_salary < 0 THEN
        SET NEW.base_salary = 0;
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `sales`
--

CREATE TABLE `sales` (
  `sale_id` int(11) NOT NULL,
  `employee_id` int(11) DEFAULT NULL,
  `client_id` int(11) DEFAULT NULL,
  `sale_date` date DEFAULT NULL,
  `sale_amount` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `sales`
--

INSERT INTO `sales` (`sale_id`, `employee_id`, `client_id`, `sale_date`, `sale_amount`) VALUES
(1, 1, 1, '2025-04-15', '5000'),
(2, 2, 2, '2025-03-12', '5000'),
(3, 3, 3, '2025-02-28', '5000'),
(4, 4, 4, '2025-01-20', '5000'),
(5, 5, 5, '2025-04-10', '5000'),
(6, 6, 6, '2025-04-01', '5000'),
(7, 7, 7, '2025-03-20', '5000'),
(8, 8, 8, '2025-02-15', '5000'),
(9, 9, 9, '2025-04-05', '5000'),
(10, 10, 10, '2025-03-25', '5000'),
(11, 11, 11, '2025-04-14', '5000'),
(12, 12, 12, '2025-04-02', '5000'),
(13, 13, 13, '2025-03-30', '5000'),
(14, 14, 14, '2025-02-18', '5000'),
(15, 15, 15, '2025-03-01', '5000');

-- --------------------------------------------------------

--
-- Stand-in structure for view `view_client_contact`
-- (See below for the actual view)
--
CREATE TABLE `view_client_contact` (
`client_id` int(11)
,`client_name` varchar(100)
,`contact_email` varchar(100)
,`contact_phone` varchar(15)
,`employee_first_name` varchar(50)
,`employee_last_name` varchar(50)
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `view_employee_department`
-- (See below for the actual view)
--
CREATE TABLE `view_employee_department` (
`employee_id` int(11)
,`first_name` varchar(50)
,`last_name` varchar(50)
,`email` varchar(100)
,`phone_number` varchar(15)
,`hire_date` date
,`department_name` varchar(100)
);

-- --------------------------------------------------------

--
-- Stand-in structure for view `view_employee_salary`
-- (See below for the actual view)
--
CREATE TABLE `view_employee_salary` (
`employee_id` int(11)
,`email` varchar(100)
,`phone_number` varchar(15)
,`hire_date` date
,`department_name` varchar(100)
,`base_salary` decimal(10,2)
);

-- --------------------------------------------------------

--
-- Structure for view `client_employee_overview`
--
DROP TABLE IF EXISTS `client_employee_overview`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `client_employee_overview`  AS SELECT `c`.`client_id` AS `client_id`, `c`.`contact_email` AS `contact_email`, `c`.`contact_phone` AS `contact_phone`, `e`.`first_name` AS `employee_first_name`, `e`.`last_name` AS `employee_last_name`, `e`.`email` AS `employee_email` FROM (`clients` `c` left join `employees` `e` on(`c`.`client_id` = `c`.`client_id`)) ;

-- --------------------------------------------------------

--
-- Structure for view `dept_employee_details`
--
DROP TABLE IF EXISTS `dept_employee_details`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `dept_employee_details`  AS SELECT `d`.`department_id` AS `department_id`, `d`.`department_name` AS `department_name`, `e`.`first_name` AS `first_name`, `e`.`last_name` AS `last_name`, `e`.`email` AS `email`, `e`.`phone_number` AS `phone_number` FROM (`departments` `d` join `employees` `e` on(`e`.`department_id` = `d`.`department_id`)) ;

-- --------------------------------------------------------

--
-- Structure for view `view_client_contact`
--
DROP TABLE IF EXISTS `view_client_contact`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_client_contact`  AS SELECT `c`.`client_id` AS `client_id`, `c`.`client_name` AS `client_name`, `c`.`contact_email` AS `contact_email`, `c`.`contact_phone` AS `contact_phone`, `e`.`first_name` AS `employee_first_name`, `e`.`last_name` AS `employee_last_name` FROM ((`clients` `c` left join `departments` `d` on(`d`.`department_id` = `d`.`department_id`)) left join `employees` `e` on(`e`.`department_id` = `d`.`department_id`)) ;

-- --------------------------------------------------------

--
-- Structure for view `view_employee_department`
--
DROP TABLE IF EXISTS `view_employee_department`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_employee_department`  AS SELECT `e`.`employee_id` AS `employee_id`, `e`.`first_name` AS `first_name`, `e`.`last_name` AS `last_name`, `e`.`email` AS `email`, `e`.`phone_number` AS `phone_number`, `e`.`hire_date` AS `hire_date`, `d`.`department_name` AS `department_name` FROM (`employees` `e` join `departments` `d` on(`e`.`department_id` = `d`.`department_id`)) ;

-- --------------------------------------------------------

--
-- Structure for view `view_employee_salary`
--
DROP TABLE IF EXISTS `view_employee_salary`;

CREATE ALGORITHM=UNDEFINED DEFINER=`root`@`localhost` SQL SECURITY DEFINER VIEW `view_employee_salary`  AS SELECT `e`.`employee_id` AS `employee_id`, `e`.`email` AS `email`, `e`.`phone_number` AS `phone_number`, `e`.`hire_date` AS `hire_date`, `d`.`department_name` AS `department_name`, `s`.`base_salary` AS `base_salary` FROM ((`employees` `e` join `departments` `d` on(`e`.`department_id` = `d`.`department_id`)) join `salaries` `s` on(`e`.`salary_id` = `s`.`salary_id`)) ;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `audit_log`
--
ALTER TABLE `audit_log`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `clients`
--
ALTER TABLE `clients`
  ADD PRIMARY KEY (`client_id`);

--
-- Indexes for table `departments`
--
ALTER TABLE `departments`
  ADD PRIMARY KEY (`department_id`),
  ADD KEY `fk_department_head` (`department_head`);

--
-- Indexes for table `employees`
--
ALTER TABLE `employees`
  ADD PRIMARY KEY (`employee_id`),
  ADD KEY `fk_salary_id` (`salary_id`),
  ADD KEY `fk_department_id` (`department_id`);

--
-- Indexes for table `payments`
--
ALTER TABLE `payments`
  ADD PRIMARY KEY (`payment_id`),
  ADD KEY `fk_employee_id_idx` (`employee_id`);

--
-- Indexes for table `projects`
--
ALTER TABLE `projects`
  ADD PRIMARY KEY (`project_id`),
  ADD KEY `fk_client_id_idx` (`client_id`);

--
-- Indexes for table `salaries`
--
ALTER TABLE `salaries`
  ADD PRIMARY KEY (`salary_id`),
  ADD KEY `employee_id_idx` (`employee_id`);

--
-- Indexes for table `sales`
--
ALTER TABLE `sales`
  ADD PRIMARY KEY (`sale_id`),
  ADD KEY `employee_id_idx` (`employee_id`),
  ADD KEY `fk_client_id_idx` (`client_id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `audit_log`
--
ALTER TABLE `audit_log`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `clients`
--
ALTER TABLE `clients`
  MODIFY `client_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `departments`
--
ALTER TABLE `departments`
  MODIFY `department_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=17;

--
-- AUTO_INCREMENT for table `employees`
--
ALTER TABLE `employees`
  MODIFY `employee_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT for table `payments`
--
ALTER TABLE `payments`
  MODIFY `payment_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `projects`
--
ALTER TABLE `projects`
  MODIFY `project_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `sales`
--
ALTER TABLE `sales`
  MODIFY `sale_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `departments`
--
ALTER TABLE `departments`
  ADD CONSTRAINT `fk_department_head` FOREIGN KEY (`department_head`) REFERENCES `employees` (`employee_id`) ON DELETE SET NULL ON UPDATE CASCADE;

--
-- Constraints for table `employees`
--
ALTER TABLE `employees`
  ADD CONSTRAINT `fk_department_id` FOREIGN KEY (`department_id`) REFERENCES `departments` (`department_id`) ON DELETE SET NULL ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_salary_id` FOREIGN KEY (`salary_id`) REFERENCES `salaries` (`salary_id`) ON DELETE SET NULL ON UPDATE CASCADE;

--
-- Constraints for table `payments`
--
ALTER TABLE `payments`
  ADD CONSTRAINT `fk_employee_id` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `projects`
--
ALTER TABLE `projects`
  ADD CONSTRAINT `fk_client_id` FOREIGN KEY (`client_id`) REFERENCES `clients` (`client_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `salaries`
--
ALTER TABLE `salaries`
  ADD CONSTRAINT `employee_id` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `sales`
--
ALTER TABLE `sales`
  ADD CONSTRAINT `fk_client_id_new` FOREIGN KEY (`client_id`) REFERENCES `clients` (`client_id`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_employee_id_new` FOREIGN KEY (`employee_id`) REFERENCES `employees` (`employee_id`) ON DELETE CASCADE ON UPDATE CASCADE;

DELIMITER $$
--
-- Events
--
CREATE DEFINER=`root`@`localhost` EVENT `recurring_event` ON SCHEDULE EVERY 1 DAY STARTS '2025-04-23 12:00:00' ON COMPLETION NOT PRESERVE ENABLE DO INSERT INTO audit_log (action_type, description, action_time)
    VALUES ('RECURRING_EVENT', 'This is a recurring event', NOW())$$

DELIMITER ;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
