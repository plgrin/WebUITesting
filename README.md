WebUITests task
### Comparison of NUnit and xUnit

#### Task Description
- Tests for Web UI Automation were implemented using NUnit and xUnit.
- The following features were implemented:
     - Parallel test execution.
     - Setup/TearDown.
     - Data Provider.
     - Test categories.
  
#### Test Execution Results
![image](https://github.com/user-attachments/assets/dc04d0cd-e84e-44b6-ae2c-8ff7f6237915)

- **NUnit**:  
  Passed: 3, Failed: 0, Skipped: 0, Total: 3  
  Duration: **22 seconds**  

- **xUnit**:  
  Passed: 3, Failed: 0, Skipped: 0, Total: 3  
  Duration: **20 seconds**

#### Conclusions
1. **Overall Test Execution:**
   - Both frameworks (NUnit and xUnit) successfully executed all 3 tests without any failures or skipped tests.
   - Both frameworks demonstrated stability and reliability during test execution.

2. **Execution Time:**
   - **xUnit** completed the tests slightly faster, with a duration of **20 seconds**.
   - **NUnit** took **22 seconds**, which is only a 2-second difference.
   - The minor time difference could be attributed to the internal workings of the test runners or the test environment setup.

3. **Parallel Execution:**
   - **NUnit** provides easier ways to implement parallelization

4. **Ease of Use:**
   - It was easier for me to work with **NUnit** framework and implement features like parallelism, Setup/TearDown, and Data Provider functionality.
