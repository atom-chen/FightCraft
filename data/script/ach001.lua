--ͨ���Ǽ��ɾ�
x100001_g_scriptId = 100001

--�¼�����
function x100001_OnEventFilter(uuid, achId, achKey)
  	return 1
end

--�ɾ���ɽ���
function x100001_OnAchievementProgress(uuid, reachNum)

  	return LuaGetDungeonAllStar(uuid)
  	
end