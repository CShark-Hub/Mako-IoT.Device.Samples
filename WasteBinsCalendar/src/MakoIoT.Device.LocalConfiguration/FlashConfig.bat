@echo off
set port=COM3
python %idf_path%\components\spiffs\spiffsgen.py --page-size 512 --obj-name-len 256 --use-magic --use-magic-len 262144 FileSystem %tmp%\config_image.bin
python %idf_path%\components\partition_table\parttool.py -p %port% write_partition -n config --input %tmp%\config_image.bin 
del %tmp%\config_image.bin 